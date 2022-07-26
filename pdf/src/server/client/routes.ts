const serverUrl = process.env.SERVER_URL as string;
const port = process.env.PORT as string;
const version = process.env.VERSION as string;

console.log(serverUrl);
console.log(port);
console.log(version);

if (serverUrl === undefined) {
    console.log('SERVER URL UNDEFINED');
    throw new Error('SERVER URL UNDEFINED');
}

if (port === undefined) {
    console.log('PORT UNDEFINED');
    throw new Error('PORT UNDEFINED');
}

if (version === undefined) {
    console.log('VERSION UNDEFINED');
    throw new Error('VERSION UNDEFINED');
}

// const routeTemplates = (version: string) => ({
//     create_pdf_download: () => `${serverUrl}/api/v${version}/create-pdf-download`,
//     get_pdf_download: (identifier: string) => `${serverUrl}/api/v${version}/download-pdf/${identifier}`,
//     get_pdf_download_pupetteer: (identifier: string) =>
//         `${serverUrl}/api/v${version}/create-pdf-download-puppeteer/${identifier}`,
// });

// class Routes {
//     public version: string;
//     constructor(version: string) {
//         this.version = version;
//     }

//     get_versioned_routes = () => {
//         return routeTemplates(this.version);
//     };
// }

// export const routeMap = new Routes(version);
// export const routeTemplatesV1 = routeMap.get_versioned_routes();
export default null;