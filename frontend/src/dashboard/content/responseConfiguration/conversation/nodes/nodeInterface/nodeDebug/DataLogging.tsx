import { sortArrayOfObjects } from "@common/utils/sorting";
import { Typography, Divider } from "@material-ui/core";
import { LineLink } from "@Palavyr-Types";
import { values } from "lodash";
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
            <ol>
                {sortArrayOfObjects(debugData).map((item: DataItem, index: number) => {
                    let key = Object.keys(item)[0];
                    let val = Object.values(item)[0];

                    if (key === "anabranchContext") {
                        val = [Object.values(val)].join(" -- ");
                    }

                    if (key === "lineMap") {
                        const res = val.map((x: LineLink) => {
                            return `from:${x.from.split("-")[0]} to:${x.to.split("-")[0]}`;
                        });
                        val = res.join("+");
                    }

                    // if (typeof val === "object" && val?.hasOwnProperty("anabranchOriginId")) {
                    //     val = [Object.values(val)].join(", ");
                    // }

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
            </ol>
            <Divider />
            <Typography align="center">Children</Typography>
            <Typography>{nodeChildren}</Typography>
        </div>
    );
};
