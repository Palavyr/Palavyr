import { PalavyrRepository } from "@api-client/PalavyrRepository";
import { SinglePurposeButton } from "@common/components/SinglePurposeButton";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import { makeStyles, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@material-ui/core";
import { CompletedConversation, ConversationUpdate } from "@Palavyr-Types";
import classNames from "classnames";
import { Align } from "dashboard/layouts/positioning/Align";
import React from "react";
import { useCallback } from "react";
import { useState, useEffect } from "react";
import { useHistory, useLocation } from "react-router-dom";
import { formatLegitTimeStamp } from "./enquiriesUtils";
import { EnquiryTimeStamp } from "./EnquiryTimeStamp";

const useStyles = makeStyles((theme) => ({
    headerCell: {
        fontWeight: "bold",
        fontSize: "16pt",
    },
    headerRow: {
        borderBottom: "3px solid black",
    },
    title: {
        padding: "1rem",
    },
    container: {
        width: "75%",
    },
    backButton: {
        marginLeft: "2rem",
    },
}));

export const ConversationReview = () => {
    const location = useLocation();
    const searchParams = new URLSearchParams(location.search);
    const conversationId = searchParams.get("conversationId") as string;

    const cls = useStyles();
    const history = useHistory();

    const repository = new PalavyrRepository();

    const [completeConversation, setCompleteConversation] = useState<CompletedConversation>([]);

    const loadConversation = useCallback(async () => {
        const completeConversation = await repository.Enquiries.getConversation(conversationId);
        setCompleteConversation(completeConversation);
    }, []);

    useEffect(() => {
        loadConversation();
    }, []);
    const numberPropertyGetter = (convo: ConversationUpdate) => {
        return convo.id;
    };

    return (
        <>
            <SinglePurposeButton
                classes={cls.backButton}
                buttonText="Return to Enquiries"
                variant="outlined"
                color="primary"
                onClick={() => {
                    history.goBack();
                }}
            />
            <Typography className={cls.title} align="center" variant="h3">
                Conversation Review
            </Typography>
            <Align>
                <TableContainer style={{ width: "95%" }} component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow className={cls.headerRow}>
                                <TableCell className={classNames(cls.headerCell)}></TableCell>
                                <TableCell className={classNames(cls.headerCell)}>Widget</TableCell>
                                <TableCell className={classNames(cls.headerCell)}>Client</TableCell>
                                <TableCell className={classNames(cls.headerCell)}>Time</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {sortByPropertyNumeric(numberPropertyGetter, completeConversation).map((convo: ConversationUpdate, index: number) => {
                                const { formattedDate, formattedTime } = formatLegitTimeStamp(convo.timeStamp);

                                return (
                                    <TableRow style={{ backgroundColor: index % 2 == 0 ? "white" : "lightgray" }} key={convo.id}>
                                        <TableCell key={convo.id + "a"}>{index + 1}</TableCell>
                                        <TableCell key={convo.id + "b"}>{convo.prompt}</TableCell>
                                        <TableCell key={convo.id + "c"}>{convo.userResponse}</TableCell>
                                        <TableCell key={convo.id + "d"}>
                                            <EnquiryTimeStamp formattedDate={formattedDate} formattedTime={formattedTime} />
                                        </TableCell>
                                    </TableRow>
                                );
                            })}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Align>
        </>
    );
};
