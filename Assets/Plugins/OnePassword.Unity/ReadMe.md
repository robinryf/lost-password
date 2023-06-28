# Overview

Unity3D adaptation of [OnePassword.NET](https://github.com/jscarle/OnePassword.NET).

Huge shoutout to the [Contributors](https://github.com/jscarle/OnePassword.NET/graphs/contributors) for making the library available!

Demo Application and usage can be found in the pilot game [LostPassword](https://github.com/robinryf/lost-password)

## Usage

1. Add the `OnePasswordUnityManager` component to one of your GameObjects that are loaded when you want to use OnePassword API.
2. Call the `EnqueueCommand` method on the manager and pass an action handler that will execute the operations against the 1Password CLI

!! Attention: The callbacks of `EnqueueCommand` run in an own Thread. So be careful when calling Unity API which is usually not Thread Safe. 
