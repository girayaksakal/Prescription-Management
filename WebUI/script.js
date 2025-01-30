const API_BASE = "https://prescriptionmanager.azurewebsites.net";

async function getPrescription() {
    const id = document.getElementById('prescriptionId').value;
    try {
        const response = await fetch(`${API_BASE}/api/v1/prescriptions/${id}`, {
            method: 'GET'
        });
        const data = await response.json();
        document.getElementById('prescriptionResult').textContent = 
            JSON.stringify(data, null, 2);
    } catch (error) {
        document.getElementById('prescriptionResult').textContent = 
            `Error: ${error.message}`;
    }
}

async function createPrescription() {
    let url = '${API_BASE}/api/v1/prescriptions';
    let options = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    };

    const patientTCID = document.getElementById('patientTCID').value.trim();
    const medicines = [];

    document.querySelectorAll('.medicine-entry').forEach(entry => {
        const medicineName = entry.querySelector('.medicine-name').value.trim();
        const medicineDosage = entry.querySelector('.medicine-dosage').value.trim();

        if (medicineName && medicineDosage) {
            medicines.push({ medicineName, medicineDosage });
        }
    });

    if (!patientTCID || medicines.length === 0) {
        alert('Please fill out all fields');
        return;
    }

    options.body = JSON.stringify({
        patientTCID,
        medicines
    });

    try {
        const response = await fetch(url, options);

        if (!response.ok) {
            throw new Error('Failed to create prescription: ${response.statusText}');
        }

        const data = await response.json();
        resultElement.textContent = JSON.stringify(data, null, 2);
    } catch (error) {
        console.log('Error:', error);
        resultElement.textContent = `Error: ${error.message}`;
    }
}

async function searchMedicines() {
    const query = document.getElementById('medicineSearch').value;
    try {
        const response = await fetch(`${API_BASE}/api/v1/medicines/autocomplete?term=${query}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            }
        });
        const results = await response.json();
        displayResults(results);
    } catch (error) {
        console.error('Search error:', error);
    }
}

function displayResults(medicines) {
    const container = document.getElementById('medicineResults');
    container.innerHTML = medicines.map(medicine => `
        <div class="card mb-2">
            <div class="card-body">
                <h6>${medicine.name}</h6>
                <p class="mb-0 text-muted">${medicine.id}</p>
            </div>
        </div>
    `).join('');
}

// Initial load check
checkHealthStatus();

async function checkHealthStatus() {
    try {
        await fetch(`${API_BASE}/health`);
        document.getElementById('statusIndicator').className = 'text-success';
        document.getElementById('statusIndicator').textContent = 'All Systems Operational';
    } catch {
        document.getElementById('statusIndicator').className = 'text-danger';
        document.getElementById('statusIndicator').textContent = 'System Outage';
    }
}

class PrescriptionDetails {
    constructor( medicineNames, medicineDosages) {
        this.medicineNames = medicineNames;
        this.medicineDosages = medicineDosages;
    }
}
