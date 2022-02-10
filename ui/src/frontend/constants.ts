import { webUrl } from "@common/client/clientUtils";

// Options
export const SUPPORTED_FONTS = ["Architects Daughter", "Fjalla One", "Source Sans Pro"];

// IDs
export const MAIN_CONTENT_DIV_ID = "main-content-div";

// Auth error states
export const INVALID_EMAIL = "invalidEmail";
export const INVALID_PASSWORD = "invalidPassword";
export const NOT_A_DEFAULT_ACCOUNT = "notADefaultAccount";

export const COULD_NOT_FIND_SERVER = "couldNotFindServer";
export const ACCOUNT_ALREADY_EXISTS = "accountAlreadyExists";

export const DEFAULT_EMAIL_USED_WITH_DIFFERENT_ACCOUNT_TYPE = "Default Email is currently used with different account type.";
export const ACCOUNT_ALREADY_EXISTS_MESSAGE = "This account already exists";

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
export const PRIVACY_POLICY = "privacypolicy";

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
export const DEFAULT_NODE_TEXT_LIST = ["Add some meaningful text.", "Asking questions periodically is a good way to keep your customers engaged."];
export const DEFAULT_NODE_TEXT = () => (Math.ceil(Math.random()) % 2 == 0 ? DEFAULT_NODE_TEXT_LIST[0] : DEFAULT_NODE_TEXT_LIST[1]);

export const defaultUrlForNewArea = (areaIdentifier: string) => `/dashboard/editor/pricing/${areaIdentifier}?tab=0`;

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
export const REMEMBER_ME_EMAIL_COOKIE_NAME = "remember-me-email-cookie";
export const REMEMBER_ME_PASSWORD_COOKIE_NAME = "remember-me-password-cookie";
export const MENU_DRAWER_STATE_COOKIE_NAME = "menu-drawer-state-cookie";
export const USE_NEW_EDITOR_COOKIE_NAME = "use-new-editor";

export const ALL_COOKIE_NAMES = [WELCOME_TOUR_COOKIE_NAME, EDITOR_TOUR_COOKIE_NAME, REMEMBER_ME_EMAIL_COOKIE_NAME, REMEMBER_ME_PASSWORD_COOKIE_NAME, MENU_DRAWER_STATE_COOKIE_NAME, USE_NEW_EDITOR_COOKIE_NAME];
