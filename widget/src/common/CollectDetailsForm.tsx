import React from "react";
import { SetStateAction } from "react";
import { Dispatch } from "react";
import { UserDetails } from "src/types";
import { FormDialog } from "./FormDialog";
import { UserDetailsDialogContent } from "./UserDetailsDialogContent";

export interface CollectDetailsFormProps {
    detailsSet: boolean;
    setDetailsSet: Dispatch<SetStateAction<boolean>>;
    userDetails: UserDetails;
    setUserDetails: Dispatch<SetStateAction<UserDetails>>;
    userDetailsDialogState: boolean;
    setUserDetailsDialogState: Dispatch<SetStateAction<boolean>>;
}

export const CollectDetailsForm = ({ detailsSet, setDetailsSet, userDetails, setUserDetails, userDetailsDialogState, setUserDetailsDialogState }: CollectDetailsFormProps) => {



    return (
        <>
            <FormDialog
                detailsSet={detailsSet}
                open={true}
                onClose={() => {
                    setUserDetailsDialogState(false);
                }}
                hideBackdrop
                headline="Provide your contact details"
                content={<UserDetailsDialogContent setDetailsSet={setDetailsSet} userDetails={userDetails} setUserDetails={setUserDetails} />}
            />
        </>
    );
};
