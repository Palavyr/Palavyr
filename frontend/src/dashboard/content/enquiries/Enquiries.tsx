import React, { useState, useCallback, useEffect, useContext } from "react";
import { Enquiries, EnquiryRow } from "@Palavyr-Types";
import { TableContainer, Paper, TableHead, TableBody, Table, makeStyles, Typography } from "@material-ui/core";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { EnquiriesTableRow } from "./EnquiriesRow";
import { EnquiriesHeader } from "./EnquiriesHeader";
import { AreaConfigurationHeader } from "@common/components/AreaConfigurationHeader";
import { ColoredButton } from "@common/components/borrowed/ColoredButton";
import { ButtonCircularProgress } from "@common/components/borrowed/ButtonCircularProgress";
import { Align } from "dashboard/layouts/positioning/Align";
import { OsTypeToggle } from "../responseConfiguration/areaSettings/enableAreas/OsTypeToggle";
import { NoDataAvailable } from "./NoDataMessage";

const useStyles = makeStyles((theme) => ({
    title: {
        padding: "1rem",
    },
    tableCell: {
        textAlign: "center",
    },
    container: {
        paddingBottom: "8rem",
        marginBottom: "8rem",
        paddingLeft: "1.5rem",
        paddingRight: "1.5rem",
    },
    delete: {
        margin: "0.4rem",
    },
}));

export const Enquires = () => {
    const { repository } = useContext(DashboardContext);
    const cls = useStyles();

    const [enquiries, setEnquiries] = useState<Enquiries>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [deleteIsLoading, setDeleteIsLoading] = useState<boolean>(false);
    const [showSeen, setShowSeen] = useState<boolean | null>(null);

    const deleteSelectedEnquiries = async (enquiries: Enquiries) => {
        setDeleteIsLoading(true);
        const seenEnquiries = enquiries.filter((x: EnquiryRow) => x.seen);
        const enqs = await repository.Enquiries.deleteSelectedEnquiries(seenEnquiries.map((x: EnquiryRow) => x.conversationId));
        setTimeout(() => {
            setEnquiries(enqs);
            setDeleteIsLoading(false);
        }, 1000);
    };

    const loadEnquiries = useCallback(async () => {
        const show = await repository.Enquiries.getShowSeenEnquiries();
        setShowSeen(show);
        const enqs = await repository.Enquiries.getEnquiries();
        setEnquiries(enqs);
        setLoading(false);
    }, []);

    const numberPropertyGetter = (enquiry: EnquiryRow) => {
        return enquiry.id;
    };

    useEffect(() => {
        loadEnquiries();
    }, [loadEnquiries]);

    const anyEnquiriesSeen = enquiries.filter((x) => x.seen).length > 0;

    const toggleShowSeen = async () => {
        const result = await repository.Enquiries.toggleShowSeenEnquiries();
        setShowSeen(result);
    };
    return (
        <div className={cls.container}>
            <AreaConfigurationHeader title="Enquiries" subtitle="Review your recent enquiries. Use the 'History' link to view the conversation. Use the 'PDF' link to view the response PDF that was sent." />
            {showSeen !== null && <OsTypeToggle controlledState={showSeen} onChange={toggleShowSeen} enabledLabel="Show Seen Enquiries" disabledLabel="Hide Seen Enquiries" />}
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <EnquiriesHeader />
                    </TableHead>
                    <TableBody>
                        {sortByPropertyNumeric(numberPropertyGetter, enquiries, true).map((enq: EnquiryRow, index: number) => {
                            if (!showSeen) {
                                if (!enq.seen) {
                                    return <EnquiriesTableRow key={[enq.conversationId, index].join("-")} index={enquiries.length - (index + 1)} enquiry={enq} setEnquiries={setEnquiries} />;
                                }
                            } else {
                                return <EnquiriesTableRow key={[enq.conversationId, index].join("-")} index={enquiries.length - (index + 1)} enquiry={enq} setEnquiries={setEnquiries} />;
                            }
                        })}
                    </TableBody>
                </Table>
            </TableContainer>
            {!loading &&
                (enquiries.length === 0 ? (
                    <NoDataAvailable text="There are no completed enquires available." />
                ) : (
                    !showSeen && enquiries.filter((x: EnquiryRow) => x.seen === false).length === 0 && <NoDataAvailable text="There are no unseen enquiries." />
                ))}
            {enquiries.length !== 0 && anyEnquiriesSeen && showSeen && (
                <Align float="right">
                    <ColoredButton classes={cls.delete} variant="outlined" color="primary" onClick={() => deleteSelectedEnquiries(enquiries)}>
                        Delete All Seen Enquiries
                        {deleteIsLoading && <ButtonCircularProgress />}
                    </ColoredButton>
                </Align>
            )}
        </div>
    );
};
