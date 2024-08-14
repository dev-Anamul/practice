# SmartCare Pro

## Overview
Smart Care Plus is an Electronic Health Register (EHR) designed to revolutionize healthcare data management by providing a comprehensive platform for capturing patient observations and clinical service data. This project aims to enhance the existing Smart Care system.
The primary purpose of Smart Care Plus is to streamline the process of capturing, managing, and accessing patient data across healthcare facilities. By digitizing health records and incorporating requested changes such as those concerning HIV testing modalities and 
diagnosis procedures, the project aims to improve the efficiency, accuracy, and accessibility of healthcare data.Smart Care Plus offers significant value to healthcare providers, administrators, and patients alike. For healthcare providers, the platform simplifies
data entry, ensures compliance with evolving healthcare standards, and facilitates better decision-making through access to comprehensive patient records. Administrators benefit from improved data management, enhanced reporting capabilities, and the ability to monitor
and address issues efficiently. Patients experience improved quality of care and communication as their health information becomes more readily available to healthcare professionals.The target audience for Smart Care Plus includes healthcare professionals such as doctors, 
nurses, and support staff working in clinical settings, as well as healthcare administrators responsible for managing healthcare data and resources. Additionally, the project serves patients who rely on efficient and accurate healthcare services facilitated by the platform.
Overall, Smart Care Plus represents a crucial advancement in healthcare technology, aiming to optimize data management processes, enhance patient care, and improve outcomes across healthcare facilities.

## Features

| Feature            | Details                               |
|--------------------|---------------------------------------|
| Client Profile Management | Comprehensive demographic management |
| Vital Anthropometry | Precise health measurements |
| HTS                | Efficient HIV testing                |
| Birth Records      | Secure birth documentation           |
| Death Records      | Accurate mortality tracking          |
| Covid              | Robust COVID management              |
| Covax              | Seamless vaccine tracking            |
| Medical Encounter - OPD | Streamlined outpatient care       |
| PEP                | Timely post-exposure prophylaxis     |
| PrEP               | Effective pre-exposure prophylaxis   |
| Under Five         | Dedicated pediatric care             |
| Medical Encounter - IPD | Integrated inpatient care        |
| Nursing Care       | Attentive nursing services           |
| IPD - Admission    | Smooth admission processes           |
| Investigation      | Thorough diagnostic investigations   |
| Pharmacy           | Reliable medication management       |
| TB Service         | Specialized tuberculosis care        |
| Surgery            | Comprehensive surgical support       |
| VMMC Service       | Quality voluntary male medical circumcision |
| Referrals          | Efficient patient referrals          |
| ART (Adult)        | Targeted antiretroviral therapy for adults |
| ART (Pediatric)    | Tailored antiretroviral therapy for children |
| Antenatal (ANC)    | Holistic antenatal care              |
| Labour & Delivery  | Safe childbirth assistance          |
| Postnatal (PNC)    | Supportive postnatal care            |
| Family Planning    | Effective family planning services   |
| Cervical Cancer    | Advanced cervical cancer screening   |

## Table of Contents
- [Getting Started](#getting-started)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)

 ## Getting Started
Welcome to Smart Care Pro! This guide will walk you through the steps to set up and run the REST API project on your local machine for development and testing purposes.

### Prerequisites
Before you begin, ensure you have the following installed on your machine:

1-.NET Core 6 SDK.

2-Entity Framework Core.

3-Visual Studio.
   
4-Microsoft SQL Server.

### Installation
#### 1.Clone the Repository:

   -git clone (repository-url)
   
   -cd (repository-directory)
   
#### 2.Restore Dependencies:

  -dotnet restore 
  
#### 3.Database Setup:

 -Configure your SQL Server connection string in the appsettings.json file.
 
 -Run EF migrations to create the database schema:
 
   -dotnet ef database update
   
 #### 4.Run the Application:
 
   -dotnet run

## Usage
Logging in to SmartCare Pro:
Users need to log in to the SmartCare Pro system using their credentials. Here's a basic example of logging in.
![carepro12](https://github.com/Excel-Technologies-Ltd/Smart-Care-Pro-Backend/assets/153589576/8e0f3a63-f8fb-4f4e-b51e-4313690f0ef3)

Accessing Patient Records:
Healthcare providers often need to access patient records to review medical history or update information. Here's how it can be done.

Prescribing Medications:
Healthcare providers may need to prescribe medications to patients. 

Performing Laboratory Investigations:
Ordering and recording laboratory investigations is another common task. Here's how it can be done:

## About Us
At Excel Technologies Limited, we are dedicated to revolutionizing healthcare through innovative technology solutions. Our team is comprised of passionate individuals with diverse backgrounds and expertise, working together to develop and maintain SmartCare Pro 
system designed to streamline healthcare data management and improve patient outcomes.

Team Composition:
Development Team: Our .NET Core backend API is crafted by a talented team of 8 Software Engineers led by an experienced Team Lead. They are committed to ensuring the robustness, scalability, and security of our system.
Frontend Team: The user interface of SmartCare Pro is developed using React by a team of 7 skilled Software Engineers, led by a proactive Team Lead. They focus on creating intuitive interfaces that enhance user experience and efficiency.
UI/UX Design Team: Our UI/UX Design Team is dedicated to creating visually appealing and user-friendly interfaces for SmartCare Pro. Their innovative designs are aimed at optimizing workflow and improving usability for healthcare professionals.
Software Quality Assurance (SQA) Team: Ensuring the reliability and quality of SmartCare Pro is the responsibility of our SQA Team. Comprising Software Engineers with a keen eye for detail, they rigorously test every aspect of the software to guarantee optimal 
performance and compliance with industry standards.

Personal Touch:
Behind SmartCare Pro is a team of passionate individuals driven by a shared vision of leveraging technology to make a positive impact in the healthcare industry. Each team member, from developers to designers to QA analysts, brings unique perspectives and skills to the
table, contributing to the success of our project.
As a cohesive unit, we collaborate closely, leveraging our collective expertise to overcome challenges and deliver a cutting-edge solution that empowers healthcare providers and improves patient care. With a commitment to excellence and a focus on innovation, we are 
proud to be at the forefront of healthcare technology, making a difference one line of code at a time.

## Privacy Policy
We safeguard personal data collected on SmartCare Pro, using it solely for healthcare services and system improvement. Information is shared with authorized personnel and third-party service providers only for system operation. We prioritize data security but cannot 
guarantee absolute protection. Policy updates will be posted on our website.

## Terms of Use
Accessing SmartCare Pro implies acceptance of our terms. Use the system lawfully and responsibly, safeguarding account credentials. Intellectual property rights belong to SmartCare Solutions. We are not liable for damages arising from system use. Changes to terms will be immediately effective upon posting.



