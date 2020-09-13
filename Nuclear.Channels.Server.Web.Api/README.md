# Nuclear Channels Web Server API 1.0

## This is a test Console Web Server API for managing your channels using remote endpoints. It relies on the Nuclear.Channels.Server.Web Class Library which contains the logic and remote endpoints which you can use to manage your Channel APIs. Unlike Console Server this library can utilize ProtectedHandler attribute which will protect decorated Channel from being managed or manipulated. To use it you must reference Nuclear.Channels.Server.Web dll.

## For this to work you must first Initialize your Channel plugins by sending physical path of your plugins directory to a web server api endpoint /PluginsChannel/InitPlugins/?serverPluginsPath= . You can check the status on /PluginsChannel/PluginsInitStatus/ web server api endpoint.

## List of Web Server Api Endpoints

#### /PluginsChannel/InitPlugins/?serverPluginsPath=
#### /PluginsChannel/PluginsInitStatus/
#### /ServerChannel/GetAllMethods/
#### /ServerChannel/GetMethodsByState/?handlerId=
#### /ServerChannel/StartMethod/?handlerId=
#### /ServerChannel/RestartMethod/?handlerId=
#### /ServerChannel/StopMethod/?handlerId=
