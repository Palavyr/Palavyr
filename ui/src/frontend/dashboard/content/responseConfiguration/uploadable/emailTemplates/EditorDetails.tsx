import { ResponseVariable } from "@Palavyr-Types";
import React from "react";

export interface UploadDetailProps {
    variableDetails: ResponseVariable[];
}

export const EditorDetails = ({ variableDetails }: UploadDetailProps) => {
    console.log(variableDetails);
    // if (variableDetails.length == 0 || variableDetails === undefined || variableDetails === null){
    //     variableDetails = [];
    // }
    return (
        <div className="alert alert-info">
            <br></br>
            <div>
                When composing the email template, you may choose to include variables that will be substituted from your account details and from the chat. Currently the supported variables are:
                <div>
                    <ul>
                        {/* {variableDetails && variableDetails.map((x: VariableDetail, index: number) => {
                            return (
                                <li key={index.toString()}>
                                    {x.pattern}: {x.details}
                                </li>
                            );
                        })} */}
                    </ul>
                </div>
            </div>
        </div>
    );
};
