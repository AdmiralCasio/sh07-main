# CENSIS AR App - University of Glasgow Scavenger Hunt

## Description

The Glasgow University Scavenger Hunt leads you on a guided scavenger hunt through Glasgow University's stunning Gilmorehill Campus, using fun clues and Augmented Reality aids to help you find your way to some of the campus' iconic location.

When you find a location, you can discover interesting insights about the location, as well as interact with the location's digital twin.

## Installing, Building and Running

To build and run the app on mobile devices, you must first install [Unity](https://unity.com/download), including its [AR Framework](https://unity.com/unity/features/arfoundation). From this point on, the steps for building and running differ depending on platform.

### Android

To build and run on Android devices, first ensure you have the latest version of Android installed.

Then, connect your device to your computer using a wired or wireless connection. You will need to enable developer mode on your Android device. The specific method to achieve this is device dependent, so please consult the relevant documentation for your device.

Once your device is connected and in developer mode, allow debugging on your device and open the folder {repo-root}/CENSIS-AR-App/ in the Unity Editor. Open the build and run menu, select 'Sample Scene', select your device, and click build and run.

The app will then build and open automatically on your Android device.

### IOS

To build and run on IOS it is somewhat more difficult.

Firstly macOS is required to access XCode to run builds and so this guide will assume you are using a mac but if you are developing on windows a virtual environment may need to be explored, however, these have proven to be difficult to work with. Assuming access to a mac and an iphone is available firstly ensure both devices are updated to the most recent software as previous versions may not be compatible. Then, if you do not have XCode installed simply visit the apple developer webpage and install the most recent version to your mac. To finish preparing the devices navigate to the privacy settings on your iphone and turn on developer mode.

Now, from the unity hub you need to install the IOS package module before opening the folder {repo-root}/CENSIS-AR-App/ in the Unity Editor. To build the project, open the build setting and firstly switch the platform to IOS before checking the developer build setting and add the 'Sample Scene'. Now click the build and run button and create a new folder name 'Builds' and XCode should be open upon completion.

Next in XCode the code should begin compiling but before we can deploy we must first navigate to the Signing and Capabilites tab, check the Enable Automatic Signing box and create and assign your team following the pop ups from the drop down menu. Now Connect your phone to the mac (a cable is most appropriate) and ensure the correct device is selected on the top bar of XCode and press run. Finally navigate to the VPN and Device Management option in the phones general setting and trust the developer and you should be able to open the app from your home page.

If any issues are encountered please refer to documentation online or this youtube video as a reference [Video Reference](https://www.youtube.com/watch?v=-Hr4-XNCf8Y&t=352s)

## Visuals

## Usage
