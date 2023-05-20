# frankenstein-testlab
MAUI Blazor application focused on executing automated test cases using Selenium and other testing libraries for the documentation of each execution of each automated test case.

How does the app work?

1. The automators will oversee creating the scripts that will be executed through the application, the application has a reference to a class library project called "TestLab" in which the entire architecture of a test project is found based on the POM pattern.

   This project is specially designed so that you can create the different applications that you want to test, it is not necessary to modify anything, as you want to test more applications you must create their respective folder with their respective POM pattern for each test application.
   
   Initially the application will be very small, but as new test scripts are added the application will grow exponentially, to expand its use within the company that is dedicated to running tests it will be necessary to package the application and distribute it locally, it is recommended to release the first version when there are at least 50 scripts, otherwise the use of the application will be very limited.
   
2. Now let's talk about the second project called "TestExecutor" which is of type .NET MAUI Blazor, it is a natively rendered web application on Windows, it is designed in a very simple way so that anyone can run a test script without having knowledge of automation, this application removes any dependencies you have on servers to run automated scripts such as Jenkins.

   The application will make use of the resources of the computer where it is installed, it will execute the script in the selected web browser and it will generate a very simple and easy to understand evidence report, the report will print the execution time, executor name, name of the test case, among other relevant data for any type of audit, all this data will be stored in a database through the API which will be described below.
   
3. Finally, it is the turn of the project called "API" which is of type ASP .NET Core Web API, oversees giving secure access to the database and storing all the important data for the .NET MAUI Blazor application to function correctly.

   When a script is executed, all the evidence of said execution will be stored here.
   
Frankenstein Test Lab architecture: ![Frankenstein Test Lab architecture](https://github.com/Frank-296/frankenstein-testlab/assets/128120916/480a42d6-743b-4419-b124-56f65e0695c9)

To see our demos visit: https://frankenstein-software.com/

Note: Preloaded test cases may fail when run due to updates companies make to their online stores.
