import { Dispatch, SetStateAction } from "react";

export const INVALID_EMAIL = "invalid_email";
export const INVALID_NAME = "invalid_name";
export const INVALID_PHONE = "invalid_phone";

export const isEmpty = (val: any) => {
    if (val === "" || val === null || val === undefined){
        return false;
    }
    return true;
}

export const checkUserName = (name: string, setStatus: Dispatch<SetStateAction<string>>) => {
    const userName = name.trim();
    if (isEmpty(userName)) {
        return true;
    } else {
        setStatus(INVALID_NAME);
        return false;
    }
}

export const checkUserEmail = (email: string, setStatus: Dispatch<SetStateAction<string>>) => {
    const userEmail = email.trim();
    if (isEmpty(userEmail)) {
        return true;
    } else {
        setStatus(INVALID_EMAIL);
        return false;
    }
}

export const checkUserPhone = (phone: string, setStatus: Dispatch<SetStateAction<string>>) => {
    const userPhone = phone.trim();
    if (isEmpty(userPhone)) {
        return true;
    } else {
        setStatus(INVALID_PHONE);
        return false;
    }
}
