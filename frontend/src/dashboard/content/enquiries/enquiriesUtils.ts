export const formatTimeStamp = (timeStamp: string) => {
    const parts = timeStamp.split("--");

    const time = parts[1].split("-");
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

    const date = parts[0].split("-");
    const months = ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec"];
    const formattedDate = `${date[1]} - ${months[parseInt(date[2]) - 1]} - ${date[0]}`;
    return { formattedDate, formattedTime };
};
