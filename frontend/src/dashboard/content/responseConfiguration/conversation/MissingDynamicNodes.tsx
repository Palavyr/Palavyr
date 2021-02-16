import React from "react"
import { Typography, Chip } from "@material-ui/core"

interface IMissingDynamicNodes {
    missingNodeTypes: string[];
}


export const MissingDynamicNodes = ({ missingNodeTypes }: IMissingDynamicNodes) => {

    return (
        <>
            <Typography align="center" style={{marginBottom: "1rem", fontWeight: "bolder"}} variant="h5">These node types are currently missing from your tree:</Typography>
            <div style={{textAlign: "center"}}>
            {
                missingNodeTypes.map(
                    (x, index) => {
                        return <Chip key={index} style={{marginRight: "0.7rem"}} label={x} color="secondary" />
                    }
                )
            }
            </div>
        </>
    )
}