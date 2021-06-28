# WordJump-Serious-Games-Project
Serious Games module project for my final year at NTU.
VESVET is an EU initiative to encourage entrepreneurial learning in higher education: This project was built in collaborating with tutors and VESVET representatives that gave feedback to encourage development of a game to meet educational criteria.

# How to Play

## 1. The Questions

First you'll need an input file of formatted questions and answers, you can find an example one [HERE](https://pastebin.com/fJ5bFy90) which you can use to make your own.

You'll need at least 4 different questions and answers to get the most from this game; the more the merrier!

![image](https://user-images.githubusercontent.com/43959652/120222178-72cd9e80-c237-11eb-87cf-a4ac6e56976e.png)


When you select "load new questions" you'll launch a file explorer that can be used to locate a valid .txt file. The game does a very simple parse check to ensure that the data is valid for gameplay, and you'll then be able to select "Play" once the file is loaded!

## 2. Gameplay

![image](https://user-images.githubusercontent.com/43959652/120222865-8b8a8400-c238-11eb-9ae8-097a51dccbed.png)

The premise is simple: jump between platforms like you're playing doodlejump. The time will deplete over time; so collect coins to progress further!

![image](https://user-images.githubusercontent.com/43959652/120223254-3bf88800-c239-11eb-9462-ee965b55c5f0.png)

Collecting coins opens up this interface. Select the correct answer to the question prompt to get additional time and score! Wrong answers _deplete_ your score, so be careful! Also make sure to answer quickly because there's a time limit here too!

Falling off the screen or running out of time will lead to you losing.

## 3. Multi-language options

If English isn't your first language, WordJump implements the *Unity Engine Localization* package that allows for easy development of alternative translations. Currently, only Welsh, German, and English are present, but I'll be happy to merge any pull requests with new translations!

## 3.1 How to Contribute (Language Options)

You'll need the following to contribute languages to this project:
* Unity 2020.3.2f1 or Later: https://unity3d.com/get-unity/download
* The project files (clone me!)
* Unity Engine Documentation for the localization system: https://docs.unity3d.com/Packages/com.unity.localization@0.4/manual/index.html

Once you've got the project installed and running locally, follow the Unity Docs to add your locale, and then open the localization table that will allow you to add your own translations. Then make a pull request to the project and I'll test and upload!

