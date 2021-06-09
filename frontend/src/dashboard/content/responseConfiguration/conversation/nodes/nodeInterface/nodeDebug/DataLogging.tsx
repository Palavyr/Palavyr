import { sortArrayOfObjects } from "@common/utils/sorting";
import { Typography, Divider } from "@material-ui/core";
import React from "react";

type DataItem = {
    [key: string]: any;
};

interface DataProps {
    data: DataItem[];
    nodeId: string;
    nodeChildren: string;
    nodeProperties: DataItem[];
}


export const DataLogging = ({ data, nodeId, nodeChildren, nodeProperties }: DataProps) => {
    return (
        <div>
            <Typography align="center">{nodeId}</Typography>
            <ul>
                {sortArrayOfObjects(data).map((item: DataItem) => {
                    const key = Object.keys(item)[0];
                    let val = Object.values(item)[0];
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
            <ul>
                {sortArrayOfObjects(nodeProperties).map((item: DataItem) => {
                    const key = Object.keys(item)[0];
                    let val = Object.values(item)[0];
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
