import { Dispatch, SetStateAction } from "react";

export const INVALID_EMAIL = "invalid_email";
export const INVALID_NAME = "invalid_name";
export const INVALID_PHONE = "invalid_phone";

export const isEmpty = (val: any) => {
    if (val === "" || val === null || val === undefined){
        return true;
    }
    return false;
}

export const checkUserName = (name: string, setStatus: Dispatch<SetStateAction<string>>) => {

    const userName = name.trim();
    if (isEmpty(userName)) {
        setStatus(INVALID_NAME);
        return false;
    } else {
        return true;
    }
}

export const checkUserEmail = (email: string, setStatus: Dispatch<SetStateAction<string>>) => {
    const userEmail = email.trim();
    if (isEmpty(userEmail)) {
        setStatus(INVALID_EMAIL);
        return false;
    } else {
        return true;
    }
}

export const checkUserPhone = (phone: string, setStatus: Dispatch<SetStateAction<string>>, maskChar: string) => {
    const userPhone = phone.trim();
    if (userPhone.includes(maskChar)) {
        setStatus(INVALID_PHONE);
        return false;
    } else {
        return true;
    }
}
