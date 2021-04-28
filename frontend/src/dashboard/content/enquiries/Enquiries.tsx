import { ApiClient } from "@api-client/Client";
import React, { useState, useCallback, useEffect } from "react";
import { Enquiries, EnquiryRow } from "@Palavyr-Types";
import { TableContainer, Paper, TableHead, TableBody, Table, makeStyles, Typography } from "@material-ui/core";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import { DashboardContext } from "dashboard/layouts/DashboardContext";
import { EnquiriesTableRow } from "./EnquiriesRow";
import { EnquiriesHeader } from "./EnquiriesHeader";

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
    },
}));

export const Enquires = () => {
    const client = new ApiClient();
    const cls = useStyles();

    const [enquiries, setEnquiries] = useState<Enquiries>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const { setIsLoading } = React.useContext(DashboardContext);

    const loadEnquiries = useCallback(async () => {
        const { data: enqs } = await client.Enquiries.getEnquiries();
        setEnquiries(enqs);
        setLoading(false);
        setIsLoading(false);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [setLoading]);

    const numberPropertyGetter = (enquiry: EnquiryRow) => {
        return enquiry.id;
    };

    useEffect(() => {
        setIsLoading(true);
        loadEnquiries();
    }, [loadEnquiries]);

    const NoDataAvailable = () => {
        return (
            <div style={{ paddingTop: "3rem" }}>
                <Typography align="center" variant="h4">
                    There are no completed enquires yet.
                </Typography>
                ;
            </div>
        );
    };

    return (
        <div className={cls.container}>
            <Typography className={cls.title} align="center" variant="h3">
                Enquiries
            </Typography>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <EnquiriesHeader />
                    </TableHead>
                    <TableBody>
                        {sortByPropertyNumeric(numberPropertyGetter, enquiries, true).map((enq: EnquiryRow, index: number) => {
                            return <EnquiriesTableRow key={index} index={enquiries.length - (index + 1)} enquiry={enq} setEnquiries={setEnquiries} />;
                        })}
                    </TableBody>
                </Table>
            </TableContainer>
            {!loading && enquiries.length === 0 && <NoDataAvailable />}
        </div>
    );
};
