# StripViewer.Examples


[Demo site](https://yassirmvcwebapp20210216111158.azurewebsites.net)


Stripviewer 

A clickable area can be created from  
- individual images ( the image becomes clickable )
- a single large image with numbers ( a button is placed on the numbers in the image )


This example is written in Dotnet Core, but the use of StripViewer is platform and language independent.
The StripViewer is a simple Html-tag that can be placed in any web-page. It uses Json-data that needs to be provided by the backend of the application.
The backend application makes the calls to a WebApi with simple Http-requests and sends the data to the WebViewer component.

Clicking of an image or button in the area results in a javascript event and returns the value of the clicked item, which then can be used in a shoppingcart.

More information and valid tokens can be obtained via info@databuilding.nl
