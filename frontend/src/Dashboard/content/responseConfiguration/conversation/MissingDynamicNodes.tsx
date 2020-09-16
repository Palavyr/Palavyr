import React from "react"
import { Typography, Chip } from "@material-ui/core"
import { RequiredDetails } from "./ConvoTree"

interface IMissingDynamicNodes {
    missingNodeTypes: Array<RequiredDetails>;
}


export const MissingDynamicNodes = ({ missingNodeTypes }: IMissingDynamicNodes) => {

    return (
        <>
            {missingNodeTypes.length > 0 && <Typography variant="h5">These node types are currently missing from your tree:</Typography>}
            {
                missingNodeTypes.map(
                    (x, index) => {
                        return <Chip key={index} label={x.prettyName} color="secondary" />
                    }
                )
            }
        </>
    )
}