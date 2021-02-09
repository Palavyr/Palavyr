import React, { useState } from "react";
import { SetStateAction } from "react";
import { Dispatch } from "react";
import { UserDetails } from "src/types";
import { checkUserEmail, checkUserName, checkUserPhone, INVALID_PHONE } from "./UserDetailsCheck";
import { UserDetailsDialog } from "./UserDetailsDialog";
import { UserDetailsDialogContent } from "./UserDetailsDialogContent";

export interface CollectDetailsFormProps {
    detailsSet: boolean;
    setDetailsSet: Dispatch<SetStateAction<boolean>>;
    userDetails: UserDetails;
    setUserDetails: Dispatch<SetStateAction<UserDetails>>;
    setUserDetailsDialogState: Dispatch<SetStateAction<boolean>>;
}

export const CollectDetailsForm = ({ detailsSet, setDetailsSet, userDetails, setUserDetails, setUserDetailsDialogState }: CollectDetailsFormProps) => {

    const [status, setStatus] = useState<string | null>(null);
    const checkUserDetailsAreSet = (userDetails: UserDetails) => {
        const userNameResult = checkUserName(userDetails.userName, setStatus);
        const userEmailResult = checkUserEmail(userDetails.userEmail, setStatus);
        const userPhoneResult = checkUserPhone(userDetails.userPhone, setStatus);

        if (!userNameResult || !userEmailResult ) {
            return false;
        }
        return true;
    };

    return (
        <>
            <UserDetailsDialog
                title="Provide your contact details"
                detailsSet={detailsSet}
                open={true}
                onClose={() => {
                    checkUserDetailsAreSet(userDetails);
                    setUserDetailsDialogState(false);
                }}
                content={<UserDetailsDialogContent status={status} setStatus={setStatus} checkUserDetailsAreSet={checkUserDetailsAreSet} setDetailsSet={setDetailsSet} userDetails={userDetails} setUserDetails={setUserDetails} />}
            />
        </>
    );
};
