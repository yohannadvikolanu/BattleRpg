# BattleRpg
This is the repository for the Test Battle Rpg application created by Yohann Advikolanu (yohann.advikolanu@gmail.com).

# Unity
Version - 2021.3.13f1

# Content
The repository contains the Unity project built to demonstrate the concepts of a Hero Battle Rpg.
Features implemented are listed below:
- ✅ Each hero has attributes like name, health, attack power, experience and level.
- ✅ A hero collection structure to keep all collected heroes and select them for battle.
- ✅ A player having the ability to collect upto 10 different heroes.
- ✅ A new player gets 3 random heroes.
- ✅ A player selects 3 heroes to enter a battle.
- ✅ When battle is initiated, battlefield is generated with 3 heroes on left and 1 enemy on right.
- ✅ Player can tap on any hero to peform an attack.
- ✅ The enemy performs an attack on a random hero, after the player's turn.
- ✅ Only 1 hero or enemy can attack at a time.
- ✅ The Battle finishes when all heroes health attributes are 0 or enemy's health attribute is 0.
- ✅ Every battle won will increase any alive hero's (health > 0) experience attribute +1.
- ✅ When battle finishes, the status of the battle is conveyed to the player (win or lose).
- ✅ Every 5th enemy battle (does not matter whether these are wins or losses) will give a random hero until player’s hero count reaches to 10.
- ✅ Every 5 experience points increases hero’s level +1 automatically.
- ✅ Each Level increase will increase hero’s attack point and health attributes by 10%.
- ✅ If a hero is tapped and hold for 3 seconds it will display info popup on both hero selection and battlefield screens.

# Testing Platform
The project has been tested in Editor and on an Android device. 
- 📱 Device Name - OnePlus 7T
- 📱 OS Name - Oxygen OS
- 📱 Android version - 11
- 📱 Build number - 11.0.9.1.HD65AA

# Things to improve on
- 🟡 Heroes are hardcoded. Dynamic generation or more heroes to pick from will be the next step.
- 🟡 Revisit the technique I've used to animate the objects. While this works, it feels a bit messy. Could be cleaner.
- 🟡 Add a bit more variability to the hurt animations to add some polish.
- 🟡 Make the battles a bit harder by making the enemy metrics tougher.

# Packages used
- 📦 Ui Rounded Corners - https://github.com/kirevdokimov/Unity-UI-Rounded-Corners

# General Notes
- 🗈 A really fun exercise! This project can be taken in a lot of different directions now.
- 🗈 Overall time spent about 18-20 hours spread over 10 days to achieve what I have.
- 🗈 I've committed all my work source control here: https://github.com/yohannadvikolanu/BattleRpg