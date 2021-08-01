import { ItemRowObject } from "../TwoItemRow/TwoItemRow";
import TimeCircle from "../../../common/svgs/misc/_24h.svg";
import SimpleIntegration from "../../../common/svgs/misc/speak_and_spell.svg";
import Ring from "../../../common/svgs/misc/ring.svg";
import Transparent from "../../../common/svgs/misc/smiley_TDFW.svg";
import Saber from "../../../common/svgs/misc/lightsaber.svg";
import Person from "../../../common/svgs/misc/_user.svg";

export const rowOne: Array<ItemRowObject> = [
    {
        title: "Seal the deal",
        text: "Use Palavyr to engage clients and help them say yes.",
        type: Ring,
    },
    {
        title: "Delight your customers",
        text: "You create the conversation, so your chats will always have human touch.",
        type: Person,
    },
];
export const rowTwo: Array<ItemRowObject> = [
    {
        title: "Easy to use",
        text: "Customers will love how easy it is to use Palavyr. And so will you.",
        type: SimpleIntegration,
    },
    {
        title: "Reduce the wait",
        text: "Palavyr helps you deliver information to your prospective customers faster than the competition.",
        type: TimeCircle,
    },
];
export const rowThree: Array<ItemRowObject> = [
    {
        title: "Be Transparent",
        text: "Use Palavyr to deliver immediate service fee estimates, so they will know exactly what they're getting, and for how much.",
        type: Transparent,
    },
    {
        title: "Trim the fat",
        text: "Review your enquiries to preemptively learn what your clients are asking about.",
        type: Saber,
    },
];
