import { Field, Form, Formik } from "formik";
import { DropFile } from "../Components/DropFile/DropFile";
import { LinkInput } from "../Components/LinkInput/LinkInput";
import { StateButton } from "../Components/StateButton/StateButton";
import { useNavigate } from "react-router";
import { GeneratePdf } from "../Services/PdfGenerator";

export function GeneratePDFView() {
    return (
        <div className='flex justify-center w-full h-full bg-secondary text-primary'>
            <div className='flex flex-col justify-center items-center w-[500px]'>
                <RequestForm />
            </div>
        </div>
    );
}

export interface PdfState {
    pdf: File;
}

const RequestForm = () => {
    var navigation = useNavigate();
    return (
        <Formik
            initialValues={{ url: '', files: [] }}
            onSubmit={(values, { setSubmitting }) => {
                if (values.url === '' && values.files.length === 0) {
                    alert("Please enter a url or upload a file");
                    setSubmitting(false);
                    return;
                }
                GeneratePdf({ files: values.files, url: values.url, options: {} })
                    .then((res: any) => {
                        const file = new File([res.data], "", { type: "application/pdf" });
                        navigation("/result", { state: { pdf: file } });
                    })
                    .finally(() => {
                        setSubmitting(false);
                    }); 
            }}
        >
            {({ isSubmitting, submitForm }) => (
                <Form>
                    <div className='text-4xl'>Use Html file</div>
                    <Field name="files" as={DropFile} />
                    <div className='text-4xl'>Or just link website</div>
                    <Field name="url" as={LinkInput} />
                    <StateButton isSubmitting={isSubmitting} submitForm={submitForm}></StateButton>
                </Form>
            )}

        </Formik>
    )
}