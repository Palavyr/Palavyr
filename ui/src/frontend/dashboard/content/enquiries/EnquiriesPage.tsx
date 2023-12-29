import React, { useState, useCallback, useEffect, useContext } from "react";
import { EnquiryResource, EnquiryResources, SelectionMap, SetState } from "@Palavyr-Types";
import { TableContainer, Paper, TableHead, TableBody, Table, makeStyles, CircularProgress } from "@material-ui/core";
import { sortByPropertyNumeric } from "@common/utils/sorting";
import { DashboardContext } from "frontend/dashboard/layouts/DashboardContext";
import { EnquiriesTableRow } from "./EnquiriesRow";
import { EnquiriesHeader } from "./EnquiriesHeader";
import { HeaderStrip } from "@common/components/HeaderStrip";
import { OsTypeToggle } from "../responseConfiguration/intentSettings/enableIntents/OsTypeToggle";
import { NoDataAvailable } from "./NoDataMessage";
import Pagination from "@material-ui/lab/Pagination";
import { EnquiryBehaviorButtons } from "./EnquiryBehaviorButtons";
import { cloneDeep } from "lodash";


const useStyles = makeStyles<{}>((theme: any) => ({
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

const paginateEnquiries = (enq: EnquiryResources, currentPage: number, pageSize: number) => {
    const start = (currentPage - 1) * pageSize;
    const end = start + pageSize;
    return enq.slice(start, end);
};

const updateTotalPages = (setTotalPages: SetState<number>, fullEnquiryList: EnquiryResources, pageSize: number, showSeen: boolean) => {
    let totalPages = 0;
    if (showSeen) {
        totalPages = Math.ceil(fullEnquiryList.length / pageSize);
    } else {
        totalPages = Math.ceil(fullEnquiryList.filter(e => !e.seen).length / pageSize);
    }
    setTotalPages(totalPages);
};

export const EnquiresPage = () => {
    const { repository, setViewName, setUnseenNotifications, unseenNotifications } = useContext(DashboardContext);
    setViewName("Enquiries");
    const cls = useStyles();

    const [fullEnquiryList, setFullEnquiryList] = useState<EnquiryResources>([]);
    const [currentPageList, setCurrentPageList] = useState<EnquiryResources>([]);
    const [selectionMap, setSelectionMap] = useState<SelectionMap>({});

    const [currentPage, setCurrentPage] = useState<number>(1);
    const [pageSize, setPageSize] = useState<number>(6);
    const [totalPages, setTotalPages] = useState<number>(0);

    const [loading, setLoading] = useState<boolean>(true);
    const [showSeen, setShowSeen] = useState<boolean | null>(null);
    const [allSelected, setAllSelected] = useState<boolean>(false);

    const filterEnqsByShowSeen = (enqs: EnquiryResources, s: boolean) => (s ? enqs : enqs.filter(e => !e.seen));

    const loadEnquiries = useCallback(async () => {
        const show = await repository.Enquiries.GetShowSeenEnquiries();
        setShowSeen(show);

        const enqs = await repository.Enquiries.GetEnquiries();
        setFullEnquiryList(enqs);

        const map = {} as SelectionMap;
        enqs.forEach(e => {
            map[e.conversationId] = false;
        });
        setSelectionMap(map);

        const availableEnqs = filterEnqsByShowSeen(enqs, show);
        const current = paginateEnquiries(availableEnqs, currentPage, pageSize);
        setCurrentPageList(current);

        setLoading(false);
    }, []);

    useEffect(() => {
        loadEnquiries();
    }, [loadEnquiries]);

    useEffect(() => {
        if (showSeen !== null) {
            const availableEnqs = filterEnqsByShowSeen(fullEnquiryList, showSeen);
            updateTotalPages(setTotalPages, availableEnqs, pageSize, showSeen);
        }
    }, [showSeen, fullEnquiryList, pageSize]);

    useEffect(() => {
        if (showSeen !== null) {
            const currentitems = filterEnqsByShowSeen(fullEnquiryList, showSeen);
            const current = paginateEnquiries(currentitems, currentPage, pageSize);
            setCurrentPageList(current);
        }
    }, [currentPage, pageSize, fullEnquiryList, showSeen]);

    const numberPropertyGetter = (enquiry: EnquiryResource) => {
        return enquiry.id;
    };

    const toggleShowSeen = async () => {
        const result = await repository.Enquiries.ToggleShowSeenEnquiries();
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
                return { conversationId: x, seen: bool };
            });
        updates.forEach(x => {
            const e = fullEnquiryList.find(y => y.conversationId === x.conversationId);
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
        const idsToDelete = Object.keys(selectionMap).filter(x => selectionMap[x]); // selected ids
        idsToDelete.forEach(x => {
            delete selectionMap[x];
        });

        setUnseenNotifications(unseenNotifications - idsToDelete.length);
        await repository.Enquiries.DeleteSelected(idsToDelete);

        const updatedEnquiries = fullEnquiryList.filter(x => !idsToDelete.includes(x.conversationId)); //
        setSelectionMap(cloneDeep(selectionMap));
        setFullEnquiryList(cloneDeep(updatedEnquiries));
    };

    return (
        <div className={cls.container}>
            <HeaderStrip title="Enquiries" subtitle="Review your recent enquiries. Use the 'History' link to view the conversation. Use the 'PDF' link to view the response PDF that was sent." />
            {showSeen !== null && <OsTypeToggle controlledState={showSeen} onChange={toggleShowSeen} enabledLabel="Show Seen Enquiries" disabledLabel="Hide Seen Enquiries" />}
            <div className={cls.ctl}>
                <Pagination count={totalPages} onChange={(_: any, page: number) => setCurrentPage(page)} variant="outlined" shape="rounded" />
                <EnquiryBehaviorButtons toggleSelectAll={toggleSelectAll} markAsSeen={markAsSeen} markAsUnseen={markAsUnSeen} deleteSelected={deleteSelected} />
            </div>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <EnquiriesHeader />
                    </TableHead>
                    <TableBody>
                        {sortByPropertyNumeric(numberPropertyGetter, currentPageList, true).map((enq: EnquiryResource, index: number) => {
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
                !showSeen && currentPageList.filter((x: EnquiryResource) => x.seen === false).length === 0 && <NoDataAvailable text="There are no unseen enquiries." />
            )}
        </div>
    );
};
