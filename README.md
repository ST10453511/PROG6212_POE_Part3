GitHub Link: https://github.com/ST10453511/PROG6212_POE_Part3.git 

YouTube Link: https://youtu.be/Uj3BLNkRFps?si=ehuQwFUGyiuHv2wQ

Contract Monthly Claim System (CMCS) - Part 3 (Final of Poe)
Student: Moegammad Yaaseen Alexander
Student Number: ST10453511
Module: PROG6212
Group 1

1.	What this project is about:
For this project, I worked on the Contract Monthly Claim System, or CMCS for short. This is the last part after starting with a simple model in Part 1 and then building a working demo in Part 2, I have now developed a full web application that runs on a real database and is ready for enterprise use.

This system helps independent contractors submit their claims each month. It doesn’t just accept claims, it also calculates payments automatically, handles approvals, and generates the final invoices. Instead of using temporary data storage like before, the application now stores everything safely on a Microsoft SQL Server database, making sure no information gets lost.

On top of this, I added a new role, the HR Administrator who has the power to manage all users and keep control of payment rates and approvals, making the system more organized and easier to handle.

Main goals I focused on:
•	Keeping data safe: I replaced the old method of using short-term memory lists with a proper SQL database through Entity Framework Core, so all data is stored long-term and securely.
•	Cutting down mistakes: I automated how hourly rates get calculated based on each lecturer’s profile, so there’s less chance of mistakes from doing it by hand.
•	Following rules: My app checks that claims don’t go over the allowed working hours by validating everything on the server side before letting anything through.
•	Giving control to HR: I made sure the HR can add or remove users themselves and create invoices for payments.

2.	How my system is set up and what I used:
I built this system using ASP.NET Core MVC, which is a way to keep things organized by separating the data, the design you see on the screen, and the behind-the-scenes logic that makes everything work.

What I’m using:
•	The main framework is ASP.NET Core 9 with C# as the programming language.
•	For storing all the data, I’m using Microsoft SQL Server, LocalDB 
•	To connect my code with the database, I rely on Entity Framework Core, and I write my models first, which later create the tables in the database automatically.
•	On the front end, the stuff you see and interact with is built with HTML5, CSS3, and Bootstrap 5, so it looks good and works well on any device.
•	For testing to make sure everything works right, I used xUnit.
•	And I did all my coding in Visual Studio 2022.

How the code works:
•	Data part (The Model):
I used AppDbContext that acts like a bridge between my C# code and the SQL database. Basically, classes like Claim and User are like blueprints that match up with tables in the database. When people upload files, instead of just saving them in a folder where they could get lost or deleted, I convert those files into a series of bytes (called byte arrays) and save them straight into the database in a special column named FileData. This way, the files stay safe and easy to access.
•	The Logic Part (The Controller):
This is where the decisions happen. Say a lecturer submits a claim; the DashboardController doesn’t just trust whatever comes in. It checks the user’s ID in the database, finds their hourly rate, and then figures out the total amount on the server side.
•	The Design Part (The View):
I use Razor Views, which are special files (.cshtml) that generate the web pages dynamically, depending on what’s happening in the code. I also use Tag Helpers like asp-for and asp-action, which help connect the HTML forms to my C# code properly, making sure everything works smoothly and securely.

3.	What I can do and how it works:
3.1.	The Lecturer:
When I want to submit a claim, all I have to do is put in the data and how many hours I worked. The system then grabs my approved hourly rate from the records and does the math for me, so I don’t have to worry about mistakes. I also must upload proof that I did the work, like a PDF or Word document, which gets stored safely in the system. As soon as I send my claim, I can track where it is through a progress bar that shows me when it's been sent, when someone is reviewing it, and finally when it's approved. If I want, I can look back at all my previous claims.

3.2.	The Programme Coordinator & Academic Manager:
The Programme Coordinator & Academic Manager get a special dashboard that shows them only the claims that have just been sent or are currently being checked. When the Programme Coordinator starts reviewing the claim, it gets locked and it can’t be changed while under review. At the end of the review, the Academic Manager can either approve it or reject it. If it’s rejected, the Academic Manager is supposed to leave a reason why, like if the hours went over the limit or something wasn’t right. 

3.3.	The HR Administrator:
HR has all the control when it comes to managing users. They can add new lecturers, update email addresses, or remove old users who aren’t active anymore. They are also the only ones who can set the lecturers hourly rate, so the lecturer can’t change it themselves. Plus, HR can pull together all the claims that got approved in a month and create a neat invoice report showing how much needs to be paid out.

4.	Improvements from Part 2 (Using feedback)

After going over the feedback from my last submission, I made some important updates to fix the issues people pointed out:

•	First, with the testing, before my tests were basic, like just checking if simple math worked (for example, making sure 1+1 equals 2). That was wrong as it didn’t really test my code. So now, I’ve made the tests align with my code. Such as making sure the system doesn’t accept negative hours worked, and that no one can claim more than 24 hours in a single day. This way, the tests catch real problems.
•	Next, the files and how you get them needed work. Originally, the files just had plain text names, which wasn’t very user-friendly. I fixed that by adding a proper “Download” button that has an icon, so it’s clear and easy to grab the files straight from the database without confusion.
•	Lastly, error handling and stability were a big issue before. The program would crash if someone entered huge numbers, throwing errors like OverflowExceptions. To stop this from happening, I set up checks to make sure inputs are valid. I made the Hourly Rate field locked, meaning people like the lecturer can’t change it (The HR can only change it) and I added rules to prevent hours worked from going over safe limits.
•	Overall, I took the feedback seriously and made sure these updates made the program stronger, easier to use, and way more reliable.

5.	How you can use this step by step

Step 1: Setting things up as HR (Admin):
•	First, you log in as the HR admin (the password is just "HR").
•	Then, find the “Add New User” button and click it.
•	Next, create a new lecturer profile. For example, you might name this person “Moe” set the username to “Moe” and give them the “Lecturer” role.
•	One important thing here is setting the lecturer’s hourly pay rate. For instance, put 500 for that. This is the number the system will use later to do all the calculations automatically.
•	After entering all the information, press the Save button, and you’ll see Moe added to the user list right away.
•	Then you redo the process by adding two more users, a programme coordinator and academic manager. (You won’t be adding an hourly rate for them)

Step 2: Submitting a claim as the lecturer:
•	After setting up Moe, log out as HR and then log back in as Moe (password “Moe” note that the password would be whatever you create it).
•	On Moe’s dashboard, you’ll see a welcome message and a big button labelled “Submit Claim.”
•	Click that.
•	The app will show Moe’s hourly rate = 500 but it’s locked, so you can’t change it, which is good since HR set it.
•	Type in how many hours Moe worked. Let’s say 10 hours, the system will automatically multiply that and show a total of 5000.
•	Next, upload a PDF document as proof of the work done.
•	Click Submit after checking everything is right.
•	Then, go to the “My Claims” section where your claim will be marked “Submitted.” You’ll also see a blue button you can click to download the file you uploaded.

Step 3: Reviewing the claim as programme coordinator:
•	Log out again and log back in as the programme coordinator.
•	On the programme coordinator’s dashboard, click the “Open Review” button.
•	Find Moes’s claim in the list and click “Start Review.” The claim status will change to “Under Review” so you know it’s being looked at.

Step 4: Approving or rejecting the claim as academic manager:
•	Log out again and log back in as the academic manager.
•	On the academic managers dashboard, click the “Open Review” button.
•	Find Moes’s claim in the list and click “Approve” or “Reject”. The claim status will change from “Under Review” to either “Approved” or “Rejected”.
•	As the academic manager you can also leave a comment

Step 5: Handling payment as HR:
•	Log back in as HR.
•	Go to the user management page and click the “Invoices” button.
•	You’ll see an invoice created automatically for Moes’s approved claim, totalling R 5,000.00.
•	Use the Print button to save this invoice as a PDF, which you can then send to payroll for payment.
•	And that’s it, as simple as that.

6.	Roles
Role – Lecturer
Functions - Submit claims, view history

Role - Programme coordinator
Functions - Reviews claims set by lecturer

Role - Academic manager
Functions - Final approval or rejection

Role - HR/Admin
Functions - Manage users, set rates, invoices

7.	How to set up and install:

What you will need first
•	Visual Studio 2022 installed and make sure you include the "ASP.NET and Web Development" option when you set it up.
•	Microsoft SQL Server ready on your computer (LocalDB) 

Connecting to the database:
By default, this app uses Visual Studio’s LocalDB, which means you can get it running without extra setup on most computers used by developers. Here’s the connection information you’ll find in the appsettings.json file: "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=CMCS_DB;Trusted_Connection=True;MultipleActiveResultSets=true"

8.	Conclusion
This is a full solution that’s ready for use in a real business environment. It now includes a real SQL database to save data, automatically handles money calculations, and enforces strong role-based controls so only the right people can access certain parts. This means the CMCS system is secure, dependable, and can grow smoothly as needed.
