import { sortArrayOfObjects } from "@common/utils/sorting";
import { Typography, Divider } from "@material-ui/core";
import React from "react";

type DataItem = {
    [key: string]: any;
};

interface DataProps {
    debugData: DataItem[];
    nodeId: string;
    nodeChildren: string;
}


export const DataLogging = ({ debugData, nodeId, nodeChildren }: DataProps) => {
    return (
        <div>
            <Typography align="center">{nodeId}</Typography>
            <ul>
                {sortArrayOfObjects(debugData).map((item: DataItem) => {
                    const key = Object.keys(item)[0];
                    let val = Object.values(item)[0];
                    if (typeof val === "object") return;
                    if (typeof val === "boolean") {
                        val = val.toString().toUpperCase();
                    }
                    return (
                        <>
                            <li>
                                <Typography>
                                    {key}: {val}
                                </Typography>
                            </li>
                        </>
                    );
                })}
            </ul>
            <Divider />
            <Typography align="center">Children</Typography>
            <Typography>{nodeChildren}</Typography>
        </div>
    );
};
