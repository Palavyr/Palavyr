// Auth error states
export const INVALID_EMAIL = "invalidEmail";
export const INVALID_PASSWORD = "invalidPassword";
export const NOT_A_DEFAULT_ACCOUNT = "notADefaultAccount";

export const INVALID_GOOGLE_TOKEN = "invalidGoogleToken";
export const NOT_A_GOOGLE_ACCOUNT = "notAGoogleAccount";
export const GOOGLE_ACCOUNT_NOT_FOUND = "googleAccountNotFound";

export const COULD_NOT_FIND_SERVER = "couldNotFindServer";
export const VERIFICATION_EMAIL_SEND = "verificationEmailSend";


// server responses (TODO: Redesign to pass server message errors directly instead of via state switch)
export const COULD_NOT_VALIDATE_GOOGLE_TOKEN = "Could not validate the Google Authentication token";
export const GOOGLE_EMAIL_USED_WITH_DIFFERENT_ACCOUNT_TYPE = "Google Email is currently used with different account type.";
export const COULD_NOT_FIND_ACCOUNT_WITH_GOOGLE = "Could not find Account with Google";
export const DEFAULT_EMAIL_USED_WITH_DIFFERENT_ACCOUNT_TYPE = "Default Email is currently used with different account type.";

export const COULD_NOT_FIND_ACCOUNT = "Could not find Account";
export const PASSWORD_DOES_NOT_MATCH =  "Password does not match.";

// Dialogs
export const REGISTER = "register";
export const TERMS_OF_SERVICE = "termsOfService";
export const CHANGE_PASSWORD = "changePassword";
export const LOGIN = "login";

// PATHS
export const DASHBOARD_HOME = "/dashboard";

// Dashboard style
export const DRAWER_WIDTH: number = 240;
export const HELP_DRAWER_WIDTH: number = 300;

