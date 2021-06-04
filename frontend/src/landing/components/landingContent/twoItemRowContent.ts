import { ItemRowObject } from "../TwoItemRow/TwoItemRow";
import TimeCircle from "../../../common/svgs/misc/_24h.svg";
import SimpleIntegration from "../../../common/svgs/misc/speak_and_spell.svg";
import Ring from "../../../common/svgs/misc/ring.svg";
import Transparent from "../../../common/svgs/misc/smiley_TDFW.svg";
import Saber from "../../../common/svgs/misc/lightsaber.svg";
import Person from "../../../common/svgs/misc/_user.svg";

export const rowOne: Array<ItemRowObject> = [
    {
        title: "Engage potential clients",
        text: "Collect all of the information that you need to engage, sort, and secure a potential client.",
        type: Ring
    },
    {
        title: "Reduce the time to sign",
        text: "Reduce turnover, reduce the time to sign, and deliver timely competitive fee estimates.",
        type: TimeCircle
    }
]
export const rowTwo: Array<ItemRowObject> = [
    {
        title: "Simple Integration",
        text: "Integrate the Palavyr.com widget into your site with as little a single line of code.",
        type: SimpleIntegration

    },
    {
        title: "Increase your capture rate",
        text: "Anticipate your client's needs ahead of time and offer a competetive fee estimate.",
        type: Person
    }
]
export const rowThree: Array<ItemRowObject> = [
    {
        title: "Be Transparent",
        text: "Automate detail collection and provide transparency for your future clients.",
        type: Transparent
    },
    {
        title: "Cut to the Chase",
        text: "Review your enquiries to preemptively learn what your clients are asking about.",
        type: Saber
    }
]
