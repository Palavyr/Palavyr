const fetchIpData = new Promise((resolve, reject) => {
    const ajax = new XMLHttpRequest();
    if (window.location.href.includes("localhost")) {
        /**
         *  Resolve with dummydata, GET call will be rejected,
         *  since ipinfos server is configured that way
         */
        resolve({ data: { country: "AU" } });
        return;
    }
    ajax.open("GET", "https://ipinfo.io/json");
    ajax.onload = () => {
        const response = JSON.parse(ajax.responseText);
        if (response) {
            resolve(response);
        } else {
            reject();
        }
    };
    ajax.onerror = reject;
    ajax.send();
});

export default fetchIpData;

//
// Example response. Use result.country
//
// {
//     "ip": "49.192.106.75",
//     "hostname": "n49-192-106-75.sun3.vic.optusnet.com.au",
//     "city": "Melbourne",
//     "region": "Victoria",
//     "country": "AU",
//     "loc": "-37.8140,144.9633",
//     "org": "AS4804 Microplex PTY LTD",
//     "postal": "3000",
//     "timezone": "Australia/Melbourne",
//     "readme": "https://ipinfo.io/missingauth"
//   }