export const serverUrl = process.env.REACT_APP_API_URL as string;

if (serverUrl) {
    console.log("API URL: " + serverUrl)
} else {
    console.log("DOTENV NOT WORKING IN CREATE REACT APP.")
}