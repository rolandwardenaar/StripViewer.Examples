# StripViewer.Examples


[Demo site](https://yassirmvcwebapp20210216111158.azurewebsites.net)


Stripviewer 

A clickable area can be created from  
- individual images ( the image becomes clickable )
- a single large image with numbers ( a button is placed on the numbers in the image )

The viewer works in combination with [the Databuilding Web Api](https://databuilding.azurewebsites.net/swagger/index.html) .

The StripViewer displays graphical representations of Car Systems together with the article information cross-linked with the original numbers and/or supplier article numbers:

- Steering parts
- Exhaust
- Axels
- Breakes etc..

This example is written in Dotnet Core, but the use of StripViewer is platform and language independent.
The StripViewer is a simple Html-tag that can be placed in any web-page. It uses Json-data that needs to be provided by the backend of the application. It is installed by placing one single javascript file in de website.
The backend application makes the calls to a WebApi with simple Http-requests and sends the data to the StripViewer component.

Clicking of an image or button in the area results in a javascript event and returns the value of the clicked item, which then can be used in a shoppingcart.


More information and valid tokens can be obtained via info@databuilding.nl

![image](https://user-images.githubusercontent.com/22129107/133269079-a86ff235-17f9-4604-b093-b58e7bfc1cc4.png)
