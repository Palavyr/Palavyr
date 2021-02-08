import React from "react";
import { SetStateAction } from "react";
import { Dispatch } from "react";
import { UserDetails } from "src/widgetCore/store/types";
import { FormDialog } from "./FormDialog";
import { UserDetailsDialogContent } from "./UserDetailsDialogContent";

export interface CollectDetailsFormProps {
    userDetails: UserDetails;
    setUserDetails: Dispatch<SetStateAction<UserDetails>>;
    userDetailsDialogState: boolean;
    setUserDetailsDialogState: Dispatch<SetStateAction<boolean>>;
}

export const CollectDetailsForm = ({userDetails, setUserDetails, userDetailsDialogState, setUserDetailsDialogState}: CollectDetailsFormProps) => {
    return (
        <FormDialog
            open={true}
            onClose={() => {
                //check if inputs are all populated
            }}
            // onFormSubmit={() => {
            //     // update dispatch store
            // }}
            hideBackdrop
            headline="Provide your contact details"
            content={<UserDetailsDialogContent userDetails={userDetails} setUserDetails={setUserDetails} />}
        />
    );
};
