using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class QAContainer : MonoBehaviour
{
    
    public Dictionary<int, string> _Questions;    
    public Dictionary<int, string> _Answers;
    public string _LastPath = "";

    // Start is called before the first frame update
    void Start()
    {
        // preserve across scene transitions
        DontDestroyOnLoad(this.gameObject);

        // init dicts
        _Questions = new Dictionary<int, string>();
        _Answers = new Dictionary<int, string>();
    }

    
    public void AddQuestionAndAnswer(string Q, string A) 
    {
        Debug.Log(Q + ": " + A);
        // adding questions and answers as pairs
        int index = _Answers.Count;
        _Questions.Add(index, Q);
        _Answers.Add(index, A);
    }

    public void DelQuestion(int index) 
    {
        // remove question without removing answer
        if (_Questions.ContainsKey(index))
            _Questions.Remove(index);
    }

    public KeyValuePair<int, string> GetRandomQuestionIndexPair() 
    {
        //Debug.Log(_Questions.Count);
        int index = _Questions.Keys.ElementAt(Random.Range(1,2));
        string question = GetQuestionAt(index);

        return new KeyValuePair<int, string>(index, question);
    }

    public string GetQuestionAt(int index)
    {
        return QueryDict(_Questions, index);
    }

    public string GetAnswerAt(int index) 
    {
        return QueryDict(_Answers, index);
    }

    public string GetAnswerExcluding(int excluding)
    {
        // Used in my case to exclude the correct answer 
        int index = excluding;
        while (index == excluding) 
        {
            index = _Answers.Keys.ElementAt(Random.Range(0, _Answers.Count));
        }

        return GetAnswerAt(index);        
    }

    public int CountQuestions() 
    {
        return _Questions.Count;
    }

    private string QueryDict(Dictionary<int, string> dict, int index) 
    {
        // queries dictionary for value at index
        string str = "";

        if (dict.ContainsKey(index))
        {
            dict.TryGetValue(index, out str);
        }

        return str;
    }

    public void ClearValues()
    {
        _Questions = new Dictionary<int, string>();
        _Answers = new Dictionary<int, string>();
    }

    public bool LoadValues(string path) 
    {
        ClearValues();
        // cleared container can now be filled using formatted streamreader
        StreamReader _InFile = new StreamReader(path);
        string contents = _InFile.ReadToEnd();
        string[] lines = contents.Split("\n"[0]);
        foreach (var line in lines)
        {
            var data = line.Split('*');
            if (data.Length != 2) continue;

            // add Q and A to container
            AddQuestionAndAnswer(data[0], data[1]);
        }

        _LastPath = path;

        // returns false if error in parsing vals
        return CountQuestions() > 3;
    }

    public void ReloadValues() 
    {
        LoadValues(_LastPath);
    }


}
