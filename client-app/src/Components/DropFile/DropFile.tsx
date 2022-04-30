import { FieldProps, useFormikContext } from 'formik';
import Dropzone from 'react-dropzone';
import "./DropFile.scss";

export const DropFile = (props: FieldProps) => {
    const formikContext = useFormikContext();
    return (
        <Dropzone onDrop={acceptedFiles => {
            formikContext.setFieldValue(formikContext.getFieldProps(props).name, acceptedFiles);
        }}>
            {({ getRootProps, getInputProps }) => (
                <div {...getRootProps()} className="drop-file-container w-full">
                    <input {...getInputProps()} />
                    <div>
                        Select HTML file
                    </div>
                </div>
            )}
        </Dropzone>
    )
}