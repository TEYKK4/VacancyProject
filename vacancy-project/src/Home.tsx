import {FormEvent, JSX, MouseEvent, SyntheticEvent, useEffect, useState} from 'react'
import Box from '@mui/material/Box'
import TextField from '@mui/material/TextField'
import Button from '@mui/material/Button'
import FormControl from '@mui/material/FormControl'
import Autocomplete from '@mui/material/Autocomplete'
import Table from "@mui/material/Table";
import Paper from "@mui/material/Paper";
import TableContainer from '@mui/material/TableContainer'
import TableHead from '@mui/material/TableHead'
import TableRow from '@mui/material/TableRow'
import TableCell from '@mui/material/TableCell'
import TableBody from '@mui/material/TableBody'

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
    tagIds: number[]
}

function Home(): JSX.Element | null {

    const [tags, setTags] = useState<TypeTag[]>([]);
    const [jobTitles, setJobTitles] = useState<TypeJobTitle[]>([]);
    const [jobseekers, setJobseekers] = useState<TypeJobseeker[]>([]);
    const [matchedJobseekers, setMatchedJobseekers] = useState<TypeJobseeker[]>([]);
    const [selectedJobTitle, setSelectedJobTitle] = useState<TypeJobTitle>();
    const [selectedTags, setSelectedTags] = useState<TypeTag[]>([]);
    const [selectedJobTitleSearch, setSelectedJobTitleSearch] = useState<TypeJobTitle>();
    const [selectedTagsSearch, setSelectedTagsSearch] = useState<TypeTag[]>([]);

    function loadTags() {
        fetch('http://localhost:5225/tag/').then(text => text.json()).then(data => {
            setTags((data as TypeTag[]))
        });
        // const response = fetch('http://localhost:5225/tag/')
        // const json: TypeTag[] = response.json()
        //
        // console.log(json);
        //
        // return json;
    }

    function loadJobTitle() {
        fetch('http://localhost:5225/job-title/').then(text => text.json()).then(data => {
            setJobTitles((data as TypeJobTitle[]))
            // console.log(data)
        });
    }

    function loadJobseekers() {
        fetch('http://localhost:5225/jobseeker/').then(text => text.json()).then(data => {
            setJobseekers((data as TypeJobseeker[]))
            // console.log(data as TypeJobseeker[])
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

    const handleRegister = (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault()

        let url: string = 'http://localhost:5225/jobseeker/'
        const data: FormData = new FormData(e.currentTarget);

        if (selectedJobTitle != undefined && selectedTags.length > 0) {
            fetch(url, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                        firstname: data.get('firstName'),
                        lastname: data.get('lastName'),
                        email: data.get('email'),
                        jobTitleId: selectedJobTitle.id,
                        tagIds: selectedTags.map((tag) => tag.id)
                    }
                )
            }).then(text => text.json()).then(async () => {
                await loadJobseekers()
            });
        }
    }

    const handleSearch = (e: MouseEvent<HTMLButtonElement>) => {
        e.preventDefault()

        let url: string = 'http://localhost:5225/jobseeker/matched'

        if (selectedJobTitleSearch != undefined && selectedTagsSearch.length > 0) {
            fetch(url, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                        jobTitleId: selectedJobTitleSearch.id,
                        tagIds: selectedTagsSearch.map((tag) => tag.id)
                    }
                )
            }).then(text => text.json()).then(data => setMatchedJobseekers((data as TypeJobseeker[])));
        }
    }

    function getTags(jobseeker: TypeJobseeker): string {
        let tagsString: string = '';
        for (const item of jobseeker.tagIds) {
            tagsString += tags.find(x => x.id == item)!.name;
            tagsString += ' ';
        }

        return tagsString
    }

    if (!tags || !jobseekers || !jobTitles) {
        return <div>loading</div>
    }

    return (
        <Box>
            <Box sx={{m: 1}} display="flex" flexDirection="row">
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
                    <FormControl fullWidth sx={{mt: 1}}>
                        <Autocomplete
                            id="jobTitle"
                            options={jobTitles}
                            getOptionLabel={(jobTitle) => jobTitle.name}
                            renderInput={(params) => <TextField {...params} label="Job title"/>}
                            disablePortal
                            onChange={                            //@ts-ignore
                                (e, selectedJobTitle: TypeJobTitle | null) => {
                                    if (selectedJobTitle != null) {
                                        setSelectedJobTitle(selectedJobTitle)
                                    }
                                }}

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
                            onChange={
                                // @ts-ignore
                                (e: SyntheticEvent<Element, Event>, selectedTags: TypeTag[]) => {
                                    if (selectedTags != null) {
                                        setSelectedTags(selectedTags);
                                    }
                                }}
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

                <TableContainer sx={{ml: 5}} component={Paper}>
                    <Table aria-label="simple table">
                        <TableHead>
                            <TableRow>
                                <TableCell>Name</TableCell>
                                <TableCell>Surname</TableCell>
                                <TableCell align="right">Email</TableCell>
                                <TableCell align="right">Job Title</TableCell>
                                <TableCell align="right">Tags</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {jobseekers.map((jobseeker) => (
                                <TableRow
                                    key={jobseeker.firstname}
                                    sx={{'&:last-child td, &:last-child th': {border: 0}}}
                                >
                                    <TableCell component="th" scope="row">{jobseeker.firstname}</TableCell>
                                    <TableCell component="th" scope="row">{jobseeker.lastname}</TableCell>
                                    <TableCell align="right">{jobseeker.email}</TableCell>
                                    <TableCell align="right">{jobTitles.find(x => x.id == jobseeker.jobTitleId)!.name}</TableCell>
                                    <TableCell align="right">{getTags(jobseeker)}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Box>

            <Box sx={{m: 1}} display="flex" flexDirection="row">
                <Box component='form' onSubmit={handleSearch} sx={{width: 1 / 4}}>
                    <FormControl fullWidth sx={{mt: 1}}>
                        <Autocomplete
                            id="jobTitle"
                            options={jobTitles}
                            getOptionLabel={(jobTitle) => jobTitle.name}
                            renderInput={(params) => <TextField {...params} label="Job title"/>}
                            disablePortal
                            onChange={
                                //@ts-ignore
                                (e, selectedJobTitle: TypeJobTitle | null) => {
                                    if (selectedJobTitle != null) {
                                        setSelectedJobTitleSearch(selectedJobTitle)
                                    }
                                }}
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
                            onChange={
                                // @ts-ignore
                                (e: SyntheticEvent<Element, Event>, selectedTags: TypeTag[]) => {
                                    if (selectedTags != null) {
                                        setSelectedTagsSearch(selectedTags);
                                    }
                                }}
                        />
                    </FormControl>
                    <Button
                        sx={{mt: 1, mb: 1}}
                        type="submit"
                        fullWidth
                        variant="contained"
                    >
                        Search
                    </Button>
                </Box>

                <TableContainer sx={{ml: 5}} component={Paper}>
                    <Table aria-label="simple table">
                        <TableHead>
                            <TableRow>
                                <TableCell>Name</TableCell>
                                <TableCell>Surname</TableCell>
                                <TableCell align="right">Email</TableCell>
                                <TableCell align="right">Job Title</TableCell>
                                <TableCell align="right">Tags</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {matchedJobseekers.map((jobseeker) => (
                                <TableRow
                                    key={jobseeker.firstname}
                                    sx={{'&:last-child td, &:last-child th': {border: 0}}}
                                >
                                    <TableCell component="th" scope="row">{jobseeker.firstname}</TableCell>
                                    <TableCell component="th" scope="row">{jobseeker.lastname}</TableCell>
                                    <TableCell align="right">{jobseeker.email}</TableCell>
                                    <TableCell align="right">{jobTitles.find(x => x.id == jobseeker.jobTitleId)!.name}</TableCell>
                                    <TableCell align="right">{getTags(jobseeker)}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Box>
        </Box>
    )
}

export default Home
