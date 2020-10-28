// / <reference types="node" />
// / <reference types="react" />
// / <reference types="react-dom" />

/*
The front end needs to send a request to an end point with a PARAM, :areaIdentifier, and this
will be sent to C# backend API which will use the accountID and areaIdentifier to retrieve from a particular endpoint

something like /endpoints/:accountId/:areaIdentifier/?authToken=23b23k5iuhi2u5b2kjb2k34uhn234ujn

Then the API will extract the accountID, the areaIdentifier and use it with the endpoint to call a DB controller
which will can nevermore (which maps the json in the db column to a class) to retrieve ONLY the data for the area
we are currently working on. This keeps the state object a little bit smaller.

So we need to first make a call when the initial page loads to retieve the area ids (and their names) in order to
render the area list on sidebar, and then also the first area in the list (a second get request).
*/

export type UUID = string;
export type AnyFunction = (...args: any[]) => any;
export type AnyVoidFunction = (...args: any[]) => void;

// Database
export type GroupRow = {
    id: number;
    groupId: string;
    parentId: string;
    groupName: string;
}
export type GroupTable = Array<GroupRow>;

export type AreaMeta = {
    areaIdentifier: string;
    groupId: string;
    areaName: string;
}

// Client
export type GroupNodeType = {
    text: string;
    optionPath: string;
    nodeId: string;
    parentId: string;
    nodeChildrenString: string;
    isRoot: boolean;
    id?: number;
    areaMeta: Array<AreaMeta>
    groupId: string;
}

export type Groups = Array<GroupNodeType>;;

export type ConvoTableRow = {
    nodeId: string;
    nodeType: string;
    fallback: boolean;
    text: string;
    nodeChildrenString: string;
    isCritical: boolean;
    isRoot: boolean;
    areaIdentifier: string;
    optionPath: Response;
};

export type Response = "Yes" | "No" | "Not Sure" | "Ok" | "Backstop" | "Yes / Not Sure" | "No / Not Sure" | "Continue" | null | any;
export type Responses = Array<Response>;


export const ValueOptionDelimiter = "|peg|";

export type ConvoNode = {
    nodeId: string;
    nodeType: string;
    fallback: boolean;
    text: string;
    nodeChildrenString: string;
    isCritical: boolean;
    isRoot: boolean;
    areaIdentifier: string;
    optionPath: Response;
    valueOptions: string, // an array, but bc of the dtabase we store as a string delimited by |peg|
    id?: number | undefined;
};

export type Conversation = Array<ConvoNode>;

export type Areas = Array<AreaTable>;

export type AreaTable = {
    // all of the data
    areaIdentifier: string;
    areaName: string;
    areaDisplayTitle: string;
    prologue: string;
    epilogue: string;
    emailTemplate: string; // an email template
    convo: Array<ConvoNode>;
    staticTables: StaticTableMetas;
    dynamicTableType: string;
    groupId: string;
    areaSpecificEmail: string;
    emailIsVerified: boolean;
    awaitingVerification: boolean;
};

export type StaticTableMetas = Array<StaticTableMeta>;
export type StaticTableRows = Array<StaticTableRow>;

export type staticTableMetaTemplate = {
    id: number | null;
    description: string;
    areaIdentifier: string;
    staticTableRows: StaticTableRows;
}

export type StaticTableMeta = staticTableMetaTemplate & {
    tableOrder: number;
};

export type StaticTableRow = {
    id: number | null;
    rowOrder: number;
    description: string;
    fee: StaticFee;
    range: boolean;
    perPerson: boolean;
    tableOrder: number;
    areaIdentifier: string;
};

export type StaticFee = {
    id: number | null;
    feeId: string;
    min: number;
    max: number;
}

type HTML = string;


export type FileLink = {
    fileId: string,
    fileName: string,
    link: string
}

export type EnquiryRow = {
    id: number;
    conversationId: string;
    responsePdfLink: FileLink;
    timeStamp: string;
    accountId: string;
    areaName: string;
    emailTemplateUsed: string;
    seen: boolean;
    name: string;
    email: string;
    phoneNumber: string;
}

export type DynamicTableMeta = {
    id: number;
    tableTag: string;
    tableType: string;
    tableId: string;
    accountId: string;
    areaId: string;
    valuesAsPaths: boolean;
    prettyName: string;
}

export type DynamicTableMetas = Array<DynamicTableMeta>;


export type AlertType = {
    title: string;
    message: string;
    link?: string;
    linktext?: string;
}


export type EmailVerificationResponse = {
    status: "Success" | "Pending" | "Failed";
    title: string;
    message: string;
}

export type AlertDetails = {
    title: string;
    message: string;
}

// Common interfaces

export interface IHaveWidth {
    width: "xs" | "sm" | "md" | "lg" | "xl";
}

// export type GoogleTokenObj = {
//     access_token: string;
//     expires_at: number;
//     expires_in: string;
//     first_issued_at: string;
//     id_token: string;
//     idpId: string;
//     login_hint: string;
//     scope: string;
//     session_state: SessionState;
//     token_type: string; // Bearer
// }

// export type ExtraQueryParams = {
//     authuser: string;
// }

// export type SessionState = {
//     extraQueryParams: ExtraQueryParams;
// }

// export type FirstName = string;
// export type LastName = string;
// export type FullName = string;
// export type EmailAddress = string;
// export type GoogleNT = {
//     Ad: FullName;
//     JJ: string;
//     Wt: EmailAddress;
//     dV: FirstName;
//     fT: LastName;
//     yT: string;
// }

// export type GoogleWC = GoogleTokenObj;
// export type GoogleProfileObj = {
//     email: EmailAddress;
//     familyName: LastName;
//     givenName: FirstName;
//     googleId: string;
//     imageUrl: string;
//     name: FullName;
// }

// export type GoogleAuthResponse = {
//     Ca: string;
//     wc: GoogleWC;
//     nt: GoogleNT;
//     googleId: string;
//     tokenObj: GoogleTokenObj;
//     tokenId: number;
//     accessToken: string;
//     profileObj: GoogleProfileObj

// }






/// declarations

// declare namespace NodeJS {
//     interface ProcessEnv {
//         readonly NODE_ENV: 'development' | 'production' | 'test';
//         readonly PUBLIC_URL: string;
//     }
// }
