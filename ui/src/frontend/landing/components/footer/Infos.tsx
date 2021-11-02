import * as React from 'react';
import EmailIcon from '@material-ui/icons/Email';


type Info = {
    icon: React.ReactElement;
    description: string;
}


export const infos: Array<Info> = [
    {
        icon: <EmailIcon />,
        description: "info.palavyr@gmail.com",
    }
];
