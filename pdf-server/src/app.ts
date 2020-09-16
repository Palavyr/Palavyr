import express, { Application, Request, Response, NextFunction } from "express";
import cors from "cors";
import pdf, { CreateOptions } from "html-pdf";


const app: Application = express();
const port: string = process.env.PORT || "5600";
console.log("starting PDF service on port: " + port)

// middleware
app.use(cors());
app.use(express.urlencoded({ extended: true }));
app.use(express.json());


const createOptions = (formId: string): CreateOptions => {
    return {
        "orientation": "portrait",
        "format": "A4",
        "border": ".5in",
        "footer": {
            "height": "14mm",
            "contents": {
                "default": `<span style="color: #444;">${formId}</span>`,
            }
        }
    }
}


///
/// This enpoint is very simple -- it receives a string and writes to a provided path.
///
app.post("/create-pdf", (req: Request, res: Response, next: NextFunction) => {

    const html = req.body.html;
    const savePath = req.body.path;
    const identifier = req.body.Id;
    const STANDARD_PAPER_OPTIONS = createOptions(identifier)

    pdf.create(html, STANDARD_PAPER_OPTIONS).toFile(savePath, (err) => {
        if (err) {
            res.send(Promise.reject())
        }
        res.send("Success");
    })
})

app.listen(port, () => console.log("Server listening..."));