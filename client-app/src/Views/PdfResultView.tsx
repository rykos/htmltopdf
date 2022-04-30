import FileSaver from 'file-saver';
import { Document, Page } from 'react-pdf/dist/esm/entry.webpack';
import { useLocation } from 'react-router';
import { PdfState } from './GeneratePDFView';

function formatBytes(bytes: number, decimals = 2) {
    if (bytes === 0) return '0 Bytes';

    const k = 1024;
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

    const i = Math.floor(Math.log(bytes) / Math.log(k));

    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
}

export function PdfResultView() {
    const state = useLocation().state as PdfState;

    function DownloadClick() {
        FileSaver.saveAs(state.pdf, "generated.pdf");
    }

    return (
        <div className='flex flex-col text-primary items-center justify-center w-full h-full bg-secondary'>
            <div className='flex w-[598px] h-[770px] overflow-hidden border border-primary'>
                <Document file={state.pdf} onLoadSuccess={() => { console.log("LoadSuccess") }}>
                    <Page pageNumber={1} />
                </Document>
            </div>
            {/* Desc */}
            <div className='flex justify-center items-center'>
                <div className='text-2xl'>
                    File size: {formatBytes(state.pdf.size)}
                </div>
                <div className='border-4 m-4 border-primary bg-dark rounded p-4 text-4xl cursor-pointer transition hover:scale-105'
                    onClick={DownloadClick}>
                    Download
                </div>
            </div>
        </div>
    )
}