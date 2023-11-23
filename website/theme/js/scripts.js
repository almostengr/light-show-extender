const JUKEBOX_ROUTE = "/jukeboxapi.php";
const ALERT_DANGER_CLASS = "alert-danger";
const ALERT_SUCCESS_CLASS = "alert-success";
const D_NONE = "d-none";

const artistElement = document.getElementById("songArtist");
const cpuTempElement = document.getElementById("cpuTemp");
const jukeboxFormElement = document.getElementById("jukeboxForm");
const sequenceNameElement = document.getElementById("sequenceName");
const codeElement = document.getElementById("code");
const nwsTempElement = document.getElementById("nwsTemp");
const showMetaDataElement = document.getElementById("showMetaData");
const songTitleElement = document.getElementById("songTitle");
const windChillElement = document.getElementById("windChill");
const lastUpdatedElement = document.getElementById("lastUpdated");
const errorsElement = document.getElementById("errors");
const onAirElement = document.getElementById("onAir");

function requestHeaders() {
    return { "Content-Type": "application/json" };
}

async function submitJukeboxRequest() {
    alertBody.classList.add(D_NONE);

    try {
        const formData = new FormData(jukeboxFormElement);
        const data = Object.fromEntries(formData);

        const response = await fetch(JUKEBOX_ROUTE, {
            method: 'POST',
            headers: requestHeaders(),
            body: JSON.stringify(data),
        });

        const result = await response.json();
        if (response.status > 299) {
            throw new Error(result.message);
        }

        alertText.innerText = result.message;
        alertBody.classList.remove(ALERT_DANGER_CLASS);
        alertBody.classList.add(ALERT_SUCCESS_CLASS);

        sequenceNameElement.selectedIndex = 0;
        codeElement.value = "";

        const alertDisplaySeconds = 5 * 1000;
        setTimeout(() => {
            alertBody.classList.add(D_NONE)
        }, alertDisplaySeconds);
    } catch (error) {
        alertText.innerText = error;
        alertBody.classList.remove(ALERT_SUCCESS_CLASS);
        alertBody.classList.add(ALERT_DANGER_CLASS);
    }

    alertBody.classList.remove(D_NONE);
}

async function getDisplayData() {
    if (songTitleElement == null) {
        return;
    }

    try {
        const response = await fetch(JUKEBOX_ROUTE, {
            method: 'GET',
            headers: requestHeaders(),
        });

        if (response.status > 299) {
            throw new Error(result.message);
        }

        let result = await response.json();

        if (result.title === "") {
            songTitleElement.innerText = "SHOW OFFLINE";
            artistElement.innerText = "Show times are listed below";
            jukeboxFormElement.classList.add(D_NONE);
            showMetaDataElement.classList.add(D_NONE);
            onAirElement.classList.add(D_NONE);
        }
        else {
            songTitleElement.innerText = result.title;
            artistElement.innerText = result.artist;
            nwsTempElement.innerText = result.nwstemp;
            windChillElement.innerText = result.windchill;
            cpuTempElement.innerText = result.cputemp;
            lastUpdatedElement.innerText = result.createdtime + " ET";
            jukeboxFormElement.classList.remove(D_NONE);
            showMetaDataElement.classList.remove(D_NONE);
            onAirElement.classList.remove(D_NONE);
        }

        errorsElement.innerText = "";
    }
    catch (errorMessage) {
        errorsElement.innerText = errorMessage;
    }
}

getDisplayData();
const delaySeconds = 1000 * 10;
setInterval(getDisplayData, delaySeconds);
