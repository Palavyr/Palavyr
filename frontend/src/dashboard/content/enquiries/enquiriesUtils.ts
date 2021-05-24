const MONTHS = ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec"];

export const formatLegitTimeStamp = (legitTimeStamp: string) => {
    // 2021-05-23T23:42:15.777717
    const parts = legitTimeStamp.split("T");

    const date = parts[0].split("-");
    const formattedDate = `${date[2]} - ${MONTHS[parseInt(date[1]) - 1]} - ${date[0]}`;

    const rawTimePreConverted = parts[1].split(".")[0];
    const rawTime = rawTimePreConverted.replaceAll(":", "-");
    const formattedTime = formatTime(rawTime);
    return { formattedDate, formattedTime };
};

export const formatTimeStamp = (timeStamp: string) => {
    //2020-09-16--12-34-10
    const parts = timeStamp.split("--");

    const formattedTime = formatTime(parts[1]);
    const formattedDate = formatDate(parts[0]);
    return { formattedDate, formattedTime };
};

const formatDate = (rawDate: string) => {
    //Date: 2021-05-23
    const date = rawDate.split("-");
    const formattedDate = `${date[1]} - ${MONTHS[parseInt(date[2]) - 1]} - ${date[0]}`;
    return formattedDate;
};

const formatTime = (rawTime: string) => {
    //Time:  12-34-10

    const time = rawTime.split("-");
    const hour = parseInt(time[0]);
    let formattedHour: number;
    let phase: string;
    if (hour < 12) {
        formattedHour = hour;
        phase = "a.m.";
    } else {
        formattedHour = hour === 12 ? hour : hour - 12;
        phase = "p.m.";
    }

    const formattedTime = `${formattedHour}: ${time[1]} ${phase}`;
    return formattedTime;
};
