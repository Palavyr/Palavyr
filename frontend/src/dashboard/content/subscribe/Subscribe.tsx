import { HelpTypes } from 'dashboard/layouts/DashboardLayout';
import * as React from 'react';


interface ISubscribe {
    setHelpType(helpType: HelpTypes): void;
}
export const Subscribe = ({ setHelpType }: ISubscribe) => {

    setHelpType("subscribe")
    return (
        <div>Do a subscribe!</div>
    )
}