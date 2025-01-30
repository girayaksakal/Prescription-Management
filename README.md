# Prescription-Management
SE4458 Final Project: Prescription and Doctor Visit Management System

## WebUI
Link: https://zealous-water-0148f0803.4.azurestaticapps.net/index.html
- Deployed from ./WebUI in github repository to Azure Static WebApps Service
- CI/CD is satisfied
- Known issue: Medicine search function is not returning expected results of API endpoint

## Backend
- Consists from 3 main components namely APIGateway, PrescriptionService and MedicationService
- Deployed to Azure WebApps Service using **Multi-container**
- Docker images built using linux/amd64 architecture
- All Dockerfiles, docker-compose.yml (local development and tests) and docker-compose.azure.yml (for production on azure) built and deployed to Azure Container Registry Service (ACR)
- API Base URL: https://prescriptionmanager.azurewebsites.net

### Endpoints of API
#### PrescriptionService
- CreatePrescription (POST): https://prescriptionmanager.azurewebsites.net/api/v1/prescriptions
- GetPrescription (GET): https://prescriptionmanager.azurewebsites.net/api/v1/{prescriptionId}
- SendNotifications (POST): https://prescriptionmanager.azurewebsites.net/api/v1/Notification/trigger
#### MedicineService
- FetchMedicineList (POST): https://prescriptionmanager.azurewebsites.net/api/v1/Medicines/refresh
- SearchMedicine (GET): https://prescriptionmanager.azurewebsites.net/api/v1/medicines/autocomplete?term={query}

## Youtube Video

## ER Diagram
![ER-Diagram](https://github.com/user-attachments/assets/ca757be1-826d-45dd-bad4-27b452afdb4f)

