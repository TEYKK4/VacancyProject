import {FormEvent, JSX, MouseEvent, useEffect, useState} from 'react'
import List from '@mui/material/List'
import Box from '@mui/material/Box'
import TextField from '@mui/material/TextField'
import Button from '@mui/material/Button'
import FormControl from '@mui/material/FormControl'
import Autocomplete from '@mui/material/Autocomplete'

type TypeTag = {
    id: number,
    name: string
}

type TypeJobTitle = {
    id: number,
    name: string
}

type TypeJobseeker = {
    id: number,
    firstname: string,
    lastname: string,
    email: string,
    jobTitleId: number,
    tags: TypeTag[]
}

function Home(): JSX.Element | null {

    async function loadTags() {
        await fetch('http://localhost:5225/tag/').then(async text => await text.json()).then(data => {
            setTags((data as TypeTag[]))
        });

        // const response = fetch('http://localhost:5225/tag/')
        // const json: TypeTag[] = response.json()
        //
        // console.log(json);
        //
        // return json;
    }
    async function loadJobTitle() {
        await fetch('http://localhost:5225/job-title/').then(async text => await text.json()).then(data => {
            setJobTitles((data as TypeJobTitle[]))
        });
    }

    async function loadJobseekers() {
        await fetch('http://localhost:5225/jobseeker/').then(async text => await text.json()).then(data => {
            setJobTitles((data as TypeJobTitle[]))
        });
    }

    useEffect(() => {
        loadJobTitle();
    }, []);

    useEffect(() => {
        loadTags();
    }, []);

    useEffect(() => {
        loadJobseekers();
    }, []);

    const [tags, setTags] = useState<TypeTag[]>([]);
    const [jobTitles, setJobTitles] = useState<TypeJobTitle[]>([]);
    const [jobseekers, setJobseekers] = useState<TypeJobseeker[]>([]);

    const handleRegister = (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault()

        let url: string = 'http://localhost:5225/jobseeker/'
        const data: FormData = new FormData(e.currentTarget);

        fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                    "id": 1,
                    "firstname": "Имя",
                    "lastname": "Фамилия",
                    "email": "example@email.com",
                    "jobTitleId": 1
                }
            )
        }).then(text => text.json()).then(() => {
            setJobseekers
        });

        console.log({
            firstName: data.get('firstName'),
            lastName: data.get('lastName'),
            age: data.get('age'),
            email: data.get('email'),
            jobTitle: data.get('jobTitle'),
            tags: data.get('tags'),
        });

        console.log(tags)
    }

    const handleSearchButton = (e: MouseEvent<HTMLButtonElement>) => {
        e.preventDefault()
    }

    return (
        <Box>
            <Box border={1} sx={{m: 1}}>
                <Box component='form' onSubmit={handleRegister} sx={{width: 1 / 4}}>
                    <TextField
                        sx={{mt: 1}}
                        id="firstName"
                        name="firstName"
                        label="First Name"
                        autoComplete="given-name"
                        fullWidth
                        required
                        autoFocus
                    />
                    <TextField
                        sx={{mt: 1}}
                        id="lastName"
                        name="lastName"
                        label="Last Name"
                        autoComplete="family-name"
                        fullWidth
                        required
                    />

                    <TextField
                        sx={{mt: 1}}
                        id="email"
                        name="email"
                        label="Email Address"
                        autoComplete="email"
                        fullWidth
                        required
                    />
                    <TextField
                        sx={{mt: 1}}
                        id="age"
                        name="age"
                        label="Age"
                        autoComplete="age"
                        fullWidth
                        required
                    />
                    <FormControl fullWidth sx={{mt: 1}}>
                        <Autocomplete
                            id="jobTitle"
                            options={jobTitles}
                            getOptionLabel={(jobTitle) => jobTitle.name}
                            renderInput={(params) => <TextField {...params} label="Job title"/>}
                            disablePortal
                        />
                    </FormControl>

                    <FormControl fullWidth sx={{mt: 1}}>
                        <Autocomplete
                            id="tags"
                            options={tags}
                            getOptionLabel={(tag) => tag.name}
                            renderInput={(params) => <TextField {...params} label="Tags"/>}
                            disablePortal
                            multiple
                            disableCloseOnSelect
                        />
                    </FormControl>
                    <Button
                        sx={{mt: 1, mb: 1}}
                        type="submit"
                        fullWidth
                        variant="contained"
                    >
                        Register
                    </Button>
                </Box>
            </Box>

            <Box border={1} sx={{m: 1}}>
                <Box component='form' sx={{width: 1 / 4}}>
                    <FormControl fullWidth sx={{mt: 1}}>
                        <Autocomplete
                            id="jobTitle"
                            options={jobTitles}
                            getOptionLabel={(jobTitle) => jobTitle.name}
                            renderInput={(params) => <TextField {...params} label="Job title"/>}
                            disablePortal
                        />
                    </FormControl>

                    <FormControl fullWidth sx={{mt: 1}}>
                        <Autocomplete
                            id="tags"
                            options={tags}
                            getOptionLabel={(tag) => tag.name}
                            renderInput={(params) => <TextField {...params} label="Tags"/>}
                            disablePortal
                            multiple
                            disableCloseOnSelect
                        />
                    </FormControl>
                    <Button
                        sx={{mt: 1, mb: 1}}
                        type="submit"
                        fullWidth
                        variant="contained"
                        onClick={handleSearchButton}
                    >
                        Search
                    </Button>
                </Box>
            </Box>

            <List>

            </List>
        </Box>
    )
}

export default Home
