import { webUrl } from "@api-client/clientUtils";

// Auth error states
export const INVALID_EMAIL = "invalidEmail";
export const INVALID_PASSWORD = "invalidPassword";
export const NOT_A_DEFAULT_ACCOUNT = "notADefaultAccount";

export const INVALID_GOOGLE_TOKEN = "invalidGoogleToken";
export const NOT_A_GOOGLE_ACCOUNT = "notAGoogleAccount";
export const GOOGLE_ACCOUNT_NOT_FOUND = "googleAccountNotFound";

export const COULD_NOT_FIND_SERVER = "couldNotFindServer";

// server responses (TODO: Redesign to pass server message errors directly instead of via state switch)
export const COULD_NOT_VALIDATE_GOOGLE_TOKEN = "Could not validate the Google Authentication token";
export const GOOGLE_EMAIL_USED_WITH_DIFFERENT_ACCOUNT_TYPE = "Google Email is currently used with different account type.";
export const COULD_NOT_FIND_ACCOUNT_WITH_GOOGLE = "Could not find Account with Google";
export const DEFAULT_EMAIL_USED_WITH_DIFFERENT_ACCOUNT_TYPE = "Default Email is currently used with different account type.";

export const COULD_NOT_FIND_ACCOUNT = "Could not find Account";
export const PASSWORD_DOES_NOT_MATCH = "Password does not match.";

// Dialogs
export const REGISTER = "register";
export const TERMS_OF_SERVICE = "termsOfService";
export const CHANGE_PASSWORD = "changePassword";
export const LOGIN = "login";
export const VERIFICATION_EMAIL_SEND = "verificationEmailSend";
export const PASSWORDS_DONT_MATCH = "passwordsDontMatch";
export const PASSWORD_TOO_SHORT = "passwordTooShort";

// PATHS
export const DASHBOARD_HOME = "/dashboard/activity";
export const CONVERSATION_REVIEW = "/dashboard/enquiries/conversation";
export const CONVERSATION_REVIEW_PARAMNAME = "conversationId";
export const RESET_PASSWORD_VERIFY = `/reset-my-password`;
export const RESET_PASSWORD_LINK = `${webUrl}${RESET_PASSWORD_VERIFY}?id=`;
export const RESET_PASSWORD_FORM = "/reset-password";
export const RESET_PASSWORD_SUCCESS = "/reset-success";
export const PURCHASE_ROUTE = "/dashboard/subscribe/purchase";

// Dashboard style
export const DRAWER_WIDTH: number = 240;
export const HELP_DRAWER_WIDTH: number = 300;

// Palavyr Configuration Node
export const DEFAULT_NODE_TEXT = "Click to add some meaningful text. Don't forget to add some personality!";

export const defaultUrlForNewArea = (areaIdentifier: string) => `/dashboard/editor/email/${areaIdentifier}?tab=0`;

// layers
export const ADD_NEW_AREA_DIALOG_BOX_zINDEX = 150;
export const ADD_NEW_AREA_BACKDROP_zINDEX = 120;
export const DASHBOARD_HEADER_TOPBAR_zINDEX = 100;
export const CHAT_DEMO_LISTBOX_zINDEX = 50;

// App Bar
export const TOPBAR_MAX_HEIGHT = 64;

// cookie names
export const WELCOME_TOUR_COOKIE_NAME = "welcome-tour-cookie";
export const EDITOR_TOUR_COOKIE_NAME = "editor-tour-cookie";

export const ALL_COOKIE_NAMES = [WELCOME_TOUR_COOKIE_NAME, EDITOR_TOUR_COOKIE_NAME];
