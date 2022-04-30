import axios from 'axios';
import { GeneratePdfRequest } from './../Models/GeneratePdfRequest';

export async function GeneratePdf(request: GeneratePdfRequest): Promise<any> {
    let fd = PdfRequestToFormData(request);

    return axios.post(process.env.REACT_APP_API_LINK + "/pdf", fd, {
        method: "POST",
        responseType: "blob"
    });
}

function PdfRequestToFormData(request: GeneratePdfRequest): FormData {
    let fd = new FormData();
    if (request.url)
        fd.append("url", request.url);
    if (request.files) {
        request.files.forEach(file => {
            fd.append("htmlFiles", file);
        });
    }
    fd.append("options", request.options);
    return fd;
}