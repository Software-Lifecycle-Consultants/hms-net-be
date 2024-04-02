# SLC Open HMS Backend API (.Net Core Web APIs)

![Hotel Management System](public/hms-cover.png)

The HMS-NET-BE is a .Net Core Web APIs backed by MySQL database that provides services and resources to  hotel operations, including reservations, room management, guest services, and more. This application provides an intuitive and efficient solution for hotel owners and staff to manage various aspects of their hotel business.

## Features

- **User Authentication**: Allow users to sign up, log in, and manage their accounts securely.

- **Room Booking**: Enable guests to view room availability, select room types, and make reservations.

- **Admin Dashboard**: Provide a comprehensive dashboard for hotel administrators to manage room inventory, reservations, and user accounts.

- **Guest Services**: Allow guests to request services, such as room cleaning, room service, and more.

- **Payment Integration**: Facilitate seamless payment processing for room bookings and services.

- **Real-time Updates**: Utilize real-time updates to notify users about reservation status and service requests.

This is a [Next.js](https://nextjs.org/) project bootstrapped with [`create-next-app`](https://github.com/vercel/next.js/tree/canary/packages/create-next-app).

## Getting Started

Follow these instructions to set up the HMS backend API project locally on your machine:

For Development:

1. Install the .NET SDK: Ensure that the .NET SDK (Software Development Kit) is installed on your machine.
You can download it from the official .NET website: https://dotnet.microsoft.com/download

2. Install Visual Studio: If you haven't already, download and install Visual Studio from the official Microsoft website: https://visualstudio.microsoft.com/
Download Visual Studio Community 2022 and install it.This will install Visual Studio Installer in your machine.
Run visual studio installer and intall following workloads at minimum. (You may opt to download your prefereces)
	a.Workloads: ASP.NET and web development
	b.Workloads:.NET desktop development
	c.Individual components:.NET 8 runtime
	
3. Clone the repository to your local machine:
 ```bash
git clone https://github.com/Software-Lifecycle-Consultants/hms-net-be.git
 ```
4. Launch Visual Studio, Cick on Open a Project or Solution, and then navigate to the directory where your .NET solution is located.
 Select the solution file (HMS.sln) and click Open.

6. Restore Dependencies: Visual Studio usually restores project dependencies automatically. However, you can manually restore them by right-clicking on the solution in the Solution Explorer, selecting Restore NuGet Packages.

7. Right click solution and build. Run the project by clicking on play button (make sure you run on Https/IIS) or press F5.

8. The APIs will be launched on https://localhost:7041/swagger/index.html

However, you will not be able to manipulate APIs without installing MYSQL in your machine. Use following instructions to install.
9. Install MySQL server and MySQL Workbench
install WSL in windows
--https://learn.microsoft.com/en-us/windows/wsl/install

install mySQL server
--https://pen-y-fan.github.io/2021/08/08/How-to-install-MySQL-on-WSL-2-Ubuntu/

-watch this in youtube
https://www.youtube.com/watch?v=DBsyCk2vZw4&t=103s

--install mySqlWorkbench and create a database that matches with information in appsettings.json default connection string data

--additional reading on how to install MQSQL Server:
https://learn.microsoft.com/en-us/windows/wsl/tutorials/wsl-database

## Contributing
We welcome contributions from the community! 

If you find any issues or have suggestions for improvement, please open an issue or submit a pull request.

If you'd like to contribute to this project, please follow these guidelines:

1. Fork the project.
2. Create a new branch for your feature or bugfix: git checkout -b feature-name
3. Make your changes and commit them: git commit -m 'Description of your changes'
4. Push your changes to your fork: git push origin feature-name
5. Create a pull request on the original repository, explaining your changes.


## License
This project is licensed under the MIT License - see the [LICENSE](https://github.com/git/git-scm.com/blob/main/MIT-LICENSE.txt) file for details.

## Acknowledgments
- Thanks to the Next.js community for creating an amazing framework for building modern web applications.
- Special thanks to all the contributors and beta testers who helped shape this project.

## Contact
For any inquiries or feedback, please contact us at hello@softwareconsultant.org  
Your feedback and contributions are welcome!


