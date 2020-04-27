# LQĐ Online Hub
A small ASP.NET Core application I created in 4 days to collect students' profiles.
Definitely not for large-scale production environment.

## Features
- Login with Azure Active Directory
- Basic CRUD operations for user profile
- Export student list to Excel files
- Role-based access control
- Custom claims mapping policy for Azure AD application

## Build the source
- Just grab .NET Core 3.1 and build it.
- Using Visual Studio 2019 is recommended, as I used this to build the app.

## Some dependencies
- Entity Framework Core for dealing with SQL database stuff.
- Bootstrap for the UI.
- Timeago.js for displaying last edit time of a profile.
- FontAwesome for just an Excel icon.
- jQuery obviously.

There might be more underlying dependencies but I won't list them here.

## Live system
[Click here](https://lqdonlinehub.azurewebsites.net) but the website is down right now and you need an Azure AD account from my school to access it xD

## Can I customize its branding and Azure AD configuration ?
Yes, just edit the appsettings.json file.

## License
Really, a license for this so-basic stuff ? Definitely no. Just grab this source code for any purposes.