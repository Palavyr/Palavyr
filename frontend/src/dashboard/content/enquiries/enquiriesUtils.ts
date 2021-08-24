export const formatLegitTimeStamp = (legitTimeStamp: string) => {
    const pt = new Date(Date.parse(legitTimeStamp + " UTC"));
    const formattedDate = pt.toDateString();
    const formattedTime = pt.toLocaleTimeString([], { hour: "numeric", minute: "2-digit", hour12: true });

    return { formattedDate, formattedTime };
};
