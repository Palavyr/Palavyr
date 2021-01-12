import { VariableDetail } from "@Palavyr-Types";
import React from "react";

export interface UploadDetailProps {
    variableDetails: VariableDetail[];
};

export const EditorDetails = ({ variableDetails }: UploadDetailProps) => {
    return (
        <div className="alert alert-info">
            <br></br>
            <div>
                When composing the email template, you may choose to include variables that will be substituted from your account details and from the chat. Currently the supported variables are:
                <div>
                    <ul>
                        {variableDetails.map((x: VariableDetail) => {
                            return (
                                <li>
                                    {x.pattern}: {x.details}
                                </li>
                            );
                        })}
                    </ul>
                </div>
            </div>
        </div>
    );
};
