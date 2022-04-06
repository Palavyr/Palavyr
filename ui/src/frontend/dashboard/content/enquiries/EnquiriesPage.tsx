import React, { useState, useCallback, useEffect, useContext } from "react";
import { Enquiries, EnquiryRow, SelectionMap } from "@Palavyr-Types";
import { TableContainer, Paper, TableHead, TableBody, Table, makeStyles, CircularProgress } from "@material-ui/core";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { EnquiriesTableRow } from "./EnquiriesRow";
import { EnquiriesHeader } from "./EnquiriesHeader";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { OsTypeToggle } from "../responseConfiguration/areaSettings/enableAreas/OsTypeToggle";
import { NoDataAvailable } from "./NoDataMessage";
import Pagination from "@material-ui/lab/Pagination";
import { EnquiryBehaviorButtons } from "./EnquiryBehaviorButtons";
import { cloneDeep } from "lodash";

const useStyles = makeStyles(theme => ({
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
    ctl: {
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
    },
    progress: {
        width: "100%",
        display: "flex",
        justifyContent: "center",
        marginTop: "3rem",
    },
}));

export const EnquiresPage = () => {
    const { repository, setViewName } = useContext(DashboardContext);
    setViewName("Enquiries");
    const cls = useStyles();

    const [fullEnquiryList, setFullEnquiryList] = useState<Enquiries>([]);
    const [currentPageList, setCurrentPageList] = useState<Enquiries>([]);
    const [selectionMap, setSelectionMap] = useState<SelectionMap>({});

    const [currentPage, setCurrentPage] = useState<number>(0);
    const [pageSize, setPageSize] = useState<number>(5);
    const [totalPages, setTotalPages] = useState<number>(0);

    const [loading, setLoading] = useState<boolean>(true);
    const [showSeen, setShowSeen] = useState<boolean | null>(null);
    const [allSelected, setAllSelected] = useState<boolean>(false);

    const paginateEnquiries = (enq: Enquiries) => {
        const start = currentPage * pageSize;
        const end = start + pageSize;
        return enq.slice(start, end);
    };

    const loadEnquiries = useCallback(async () => {
        const show = await repository.Enquiries.getShowSeenEnquiries();
        setShowSeen(show);

        const enqs = await repository.Enquiries.getEnquiries();
        setFullEnquiryList(enqs);

        const map = {} as SelectionMap;
        enqs.forEach(e => {
            map[e.conversationId] = false;
        });
        setSelectionMap(map);

        const current = paginateEnquiries(enqs);
        setCurrentPageList(current);

        setLoading(false);
    }, []);

    const updateTotalPages = () => {
        let totalPages = 0;
        if (showSeen) {
            totalPages = Math.ceil(fullEnquiryList.length / pageSize);
        } else {
            totalPages = Math.ceil(fullEnquiryList.filter(e => !e.seen).length / pageSize);
        }
        setTotalPages(totalPages);
    };

    useEffect(() => {
        updateTotalPages();
    }, [showSeen, fullEnquiryList, pageSize]);

    const handlePageChange = (event: any, page: number) => {
        setCurrentPage(page);
    };

    useEffect(() => {
        const current = paginateEnquiries(fullEnquiryList);
        setCurrentPageList(current);
    }, []);

    const numberPropertyGetter = (enquiry: EnquiryRow) => {
        return enquiry.id;
    };

    useEffect(() => {
        loadEnquiries();
    }, [loadEnquiries]);

    const toggleShowSeen = async () => {
        const result = await repository.Enquiries.toggleShowSeenEnquiries();
        setShowSeen(result);
    };

    const toggleSelected = (conversation: string) => {
        selectionMap[conversation] = !selectionMap[conversation];
        setSelectionMap(cloneDeep(selectionMap));
    };

    const toggleSelectAll = () => {
        Object.keys(selectionMap).forEach(x => {
            selectionMap[x] = !allSelected;
        });

        setAllSelected(!allSelected);
        setSelectionMap(cloneDeep(selectionMap));
    };

    const markAs = async (bool: boolean) => {
        const updates = Object.keys(selectionMap)
            .filter(x => selectionMap[x])
            .map(x => {
                selectionMap[x] = !bool;
                return { ConversationId: x, Seen: bool };
            });
        updates.forEach(x => {
            const e = fullEnquiryList.find(y => y.conversationId === x.ConversationId);
            if (e) {
                e.seen = bool;
            }
        });
        await repository.Enquiries.UpdateSeen(updates);
        setSelectionMap(cloneDeep(selectionMap));
        setFullEnquiryList(cloneDeep(fullEnquiryList));
    };

    const markAsSeen = async () => {
        await markAs(true);
    };

    const markAsUnSeen = async () => {
        await markAs(false);
    };

    const deleteSelected = async () => {
        const idsToDelete = Object.keys(selectionMap).filter(x => selectionMap[x]);
        const updatedEnquiries = fullEnquiryList.filter(x => !idsToDelete.includes(x.conversationId));

        await repository.Enquiries.DeleteSelected(idsToDelete);
        setSelectionMap(cloneDeep(selectionMap));
        setFullEnquiryList(cloneDeep(updatedEnquiries));
    };

    return (
        <div className={cls.container}>
            <HeaderStrip title="Enquiries" subtitle="Review your recent enquiries. Use the 'History' link to view the conversation. Use the 'PDF' link to view the response PDF that was sent." />
            {showSeen !== null && <OsTypeToggle controlledState={showSeen} onChange={toggleShowSeen} enabledLabel="Show Seen Enquiries" disabledLabel="Hide Seen Enquiries" />}
            <div className={cls.ctl}>
                <Pagination count={totalPages} onChange={handlePageChange} variant="outlined" shape="rounded" />
                <EnquiryBehaviorButtons toggleSelectAll={toggleSelectAll} markAsSeen={markAsSeen} markAsUnseen={markAsUnSeen} deleteSelected={deleteSelected} />
            </div>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <EnquiriesHeader />
                    </TableHead>
                    <TableBody>
                        {sortByPropertyNumeric(numberPropertyGetter, currentPageList, true).map((enq: EnquiryRow, index: number) => {
                            if (!showSeen) {
                                if (!enq.seen) {
                                    return (
                                        <EnquiriesTableRow
                                            key={[enq.conversationId, index].join("-")}
                                            enquiry={enq}
                                            toggleSelected={toggleSelected}
                                            markAsSeen={markAsSeen}
                                            selected={selectionMap[enq.conversationId]}
                                        />
                                    );
                                }
                            } else {
                                return (
                                    <EnquiriesTableRow
                                        key={[enq.conversationId, index].join("-")}
                                        enquiry={enq}
                                        toggleSelected={toggleSelected}
                                        markAsSeen={markAsSeen}
                                        selected={selectionMap[enq.conversationId]}
                                    />
                                );
                            }
                        })}
                    </TableBody>
                </Table>
            </TableContainer>
            {loading ? (
                <div className={cls.progress}>
                    <CircularProgress />
                </div>
            ) : fullEnquiryList.length === 0 ? (
                <NoDataAvailable text="There are no completed enquires available." />
            ) : (
                !showSeen && currentPageList.filter((x: EnquiryRow) => x.seen === false).length === 0 && <NoDataAvailable text="There are no unseen enquiries." />
            )}
        </div>
    );
};
